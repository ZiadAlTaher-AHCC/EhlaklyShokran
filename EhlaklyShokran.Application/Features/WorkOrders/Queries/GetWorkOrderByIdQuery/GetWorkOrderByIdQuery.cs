using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Domain.Common.Results;

namespace EhlaklyShokran.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : ICachedQuery<Result<WorkOrderDto>>
{
    public string CacheKey => $"work-order:{WorkOrderId}";
    public string[] Tags => ["work-order"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}