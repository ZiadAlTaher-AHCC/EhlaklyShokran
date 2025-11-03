using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.BarberTasks;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.WorkOrders.Events;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.UpdateWorkOrderBarberTasks;

public class UpdateWorkOrderBarberTasksCommandHandler(
    ILogger<UpdateWorkOrderBarberTasksCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache,
    IWorkOrderPolicy workOrderValidator)
    : IRequestHandler<UpdateWorkOrderBarberTasksCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateWorkOrderBarberTasksCommand command, CancellationToken ct)
    {
        var workOrder = await context.WorkOrders
            .Include(w => w.BarberTasks)
            .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        if (command.BarberTaskIds.Length == 0)
        {
            logger.LogError("Empty BarberTaskIds list submitted.");

            return BarberTaskErrors.AtLeastOneBarberTaskIsRequired;
        }

        var requestedTasks = await context.BarberTasks
            .Where(t => command.BarberTaskIds.Contains(t.Id))
            .ToListAsync(ct);

        if (requestedTasks.Count != command.BarberTaskIds.Length)
        {
            var missingIds = command.BarberTaskIds.Except(requestedTasks.Select(t => t.Id)).ToArray();

            logger.LogError("One or more BarberTasks not found. {ids}", string.Join(", ", missingIds));

            return ApplicationErrors.BarberTaskNotFound;
        }

        var clearExistingResult = workOrder.ClearBarberTasks();

        if (clearExistingResult.IsError)
        {
            return clearExistingResult;
        }

        foreach (var task in requestedTasks)
        {
            var addBarberTaskResult = workOrder.AddBarberTask(task);

            if (addBarberTaskResult.IsError)
            {
                return addBarberTaskResult;
            }
        }

        var totalDuration = TimeSpan.FromMinutes(requestedTasks.Sum(x => (int)x.EstimatedDurationInMins));

        var newEndAt = workOrder.StartAtUtc + totalDuration;

        // Business validations
        if (workOrderValidator.IsOutsideOperatingHours(workOrder.StartAtUtc, totalDuration))
        {
            return Error.Conflict("WorkOrder_Outside_OperatingHours", "WorkOrder timing exceeds business hours.");
        }

        var spotCheckResult = await workOrderValidator.CheckSpotAvailabilityAsync(
            workOrder.Spot,
            workOrder.StartAtUtc,
            newEndAt,
            excludeWorkOrderId: workOrder.Id,
            ct: ct);

        if (spotCheckResult.IsError)
        {
            return spotCheckResult.Errors;
        }

        if (await workOrderValidator.IsLaborOccupied(workOrder.LaborId, workOrder.Id, workOrder.StartAtUtc, newEndAt))
        {
            return ApplicationErrors.LaborOccupied;
        }

        workOrder.UpdateTiming(workOrder.StartAtUtc, newEndAt);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await cache.RemoveByTagAsync("work-order", ct);

        return Result.Updated;
    }
}