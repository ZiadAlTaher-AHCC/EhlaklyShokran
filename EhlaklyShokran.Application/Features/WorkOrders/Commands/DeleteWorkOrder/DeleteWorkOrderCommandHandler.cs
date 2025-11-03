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

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public class DeleteWorkOrderCommandHandler(
    ILogger<DeleteWorkOrderCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache
    )
    : IRequestHandler<DeleteWorkOrderCommand, Result<Deleted>>
{
    private readonly ILogger<DeleteWorkOrderCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<Deleted>> Handle(DeleteWorkOrderCommand command, CancellationToken ct)
    {
        var workOrder = await _context.WorkOrders
            .FirstOrDefaultAsync(a => a.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            _logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        if (workOrder.State is not WorkOrderState.Scheduled)
        {
            _logger.LogError(
                "Deletion failed: only 'Scheduled' or 'Confirmed' WorkOrders can be deleted. Current status: {Status}",
                workOrder.State);

            return WorkOrderErrors.Readonly;
        }

        _context.WorkOrders.Remove(workOrder);

        await _context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await _cache.RemoveByTagAsync("work-order", ct);

        return Result.Deleted;
    }
}