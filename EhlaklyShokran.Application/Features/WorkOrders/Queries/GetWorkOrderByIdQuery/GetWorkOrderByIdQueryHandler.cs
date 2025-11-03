using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Application.Features.WorkOrders.Mappers;
using EhlaklyShokran.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public class GetWorkOrderByIdQueryHandler(
    ILogger<GetWorkOrderByIdQueryHandler> logger,
    IApplicationDbContext context
    )
    : IRequestHandler<GetWorkOrderByIdQuery, Result<WorkOrderDto>>
{
    private readonly ILogger<GetWorkOrderByIdQueryHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<WorkOrderDto>> Handle(GetWorkOrderByIdQuery query, CancellationToken ct)
    {
        var workOrder = await _context.WorkOrders.AsNoTracking()
                                            .Include(a => a.BarberTasks)
                                               .ThenInclude(a => a.Cosmetics)
                                            .Include(a => a.Labor)
                                            .Include(v => v.Customer)
                                            .Include(a => a.Invoice)
                                        .FirstOrDefaultAsync(a => a.Id == query.WorkOrderId, ct);

        if (workOrder is null)
        {
            _logger.LogWarning("WorkOrder with id {WorkOrderId} was not found", query.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        return workOrder.ToDto();
    }
}