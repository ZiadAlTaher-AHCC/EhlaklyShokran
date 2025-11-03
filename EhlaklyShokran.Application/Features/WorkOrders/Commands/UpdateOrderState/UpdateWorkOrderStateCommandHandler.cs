using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders;
using EhlaklyShokran.Domain.Workorders.Enums;
using EhlaklyShokran.Domain.WorkOrders.Events;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.UpdateOrderState;

public class UpdateWorkOrderStateCommandHandler(
    ILogger<UpdateWorkOrderStateCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache,
    TimeProvider dateTime
    )
    : IRequestHandler<UpdateWorkOrderStateCommand, Result<Updated>>
{
    private readonly ILogger<UpdateWorkOrderStateCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;
    private readonly TimeProvider _dateTime = dateTime;

    public async Task<Result<Updated>> Handle(UpdateWorkOrderStateCommand command, CancellationToken ct)
    {
        var workOrder = await _context.WorkOrders
            .FirstOrDefaultAsync(a => a.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            _logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        if (workOrder.StartAtUtc > _dateTime.GetUtcNow())
        {
            _logger.LogError("State transition for WorkOrder Id '{WorkOrderId}` is not allowed before the work order�s scheduled start time.", command.WorkOrderId);

            return WorkOrderErrors.StateTransitionNotAllowed(workOrder.StartAtUtc);
        }

        var updateStatusResult = workOrder.UpdateState(command.State);

        if (updateStatusResult.IsError)
        {
            _logger.LogError("Failed to update status: {Error}", updateStatusResult.TopError.Description);

            return updateStatusResult.Errors;
        }

        if (command.State == WorkOrderState.Completed)
        {
            workOrder.AddDomainEvent(new WorkOrderCompleted { WorkOrderId = command.WorkOrderId });
        }

        await _context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await _cache.RemoveByTagAsync("work-order", ct);

        return Result.Updated;
    }
}