using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.WorkOrders.Events;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.RelocateWorkOrder;

public class RelocateWorkOrderCommandHandler(
    ILogger<RelocateWorkOrderCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache,
    IWorkOrderPolicy WorkOrderValidator
    )
    : IRequestHandler<RelocateWorkOrderCommand, Result<Updated>>
{
    private readonly ILogger<RelocateWorkOrderCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;
    private readonly IWorkOrderPolicy _appointmentValidator = WorkOrderValidator;

    public async Task<Result<Updated>> Handle(RelocateWorkOrderCommand command, CancellationToken ct)
    {
        var workOrder = await _context.WorkOrders
            .Include(a => a.BarberTasks)
            .Include(a => a.Labor)
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            _logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        var duration = workOrder.EndAtUtc.Subtract(workOrder.StartAtUtc).Duration();

        var endAt = command.NewStartAt.Add(duration);

        var checkSpotAvailabilityResult = await _appointmentValidator.CheckSpotAvailabilityAsync(
            workOrder.Spot,
            command.NewStartAt,
            endAt,
            excludeWorkOrderId: workOrder.Id,
            ct);

        if (checkSpotAvailabilityResult.IsError)
        {
            _logger.LogError("Spot: {Spot} is not available.", workOrder.Spot.ToString());

            return checkSpotAvailabilityResult.Errors;
        }

        if (await _appointmentValidator.IsLaborOccupied(workOrder.LaborId, command.WorkOrderId, command.NewStartAt, endAt))
        {
            _logger.LogError("Labor with Id '{LaborId}' is already occupied during the requested time.", workOrder.LaborId);

            return ApplicationErrors.LaborOccupied;
        }

        if (await _appointmentValidator.IsCustomerAlreadyScheduled(workOrder.CustomerId, command.NewStartAt, endAt, command.WorkOrderId))
        {
            _logger.LogError("Customer with Id '{CustomerId}' already has an overlapping WorkOrder.", workOrder.CustomerId);

            return ApplicationErrors.CustomerSchedulingConflict;
        }

        var updateTimingResult = workOrder.UpdateTiming(command.NewStartAt, endAt);

        if (updateTimingResult.IsError)
        {
            _logger.LogError("Failed to update timing: {Error}", updateTimingResult.TopError.Description);

            return updateTimingResult.Errors;
        }

        var updateSpotResult = workOrder.UpdateSpot(command.NewSpot);

        if (updateTimingResult.IsError)
        {
            _logger.LogError("Failed to update Spot: {Error}", updateSpotResult.TopError.Description);

            return updateTimingResult.Errors;
        }

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await _context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await _cache.RemoveByTagAsync("work-order", ct);

        return Result.Updated;
    }
}