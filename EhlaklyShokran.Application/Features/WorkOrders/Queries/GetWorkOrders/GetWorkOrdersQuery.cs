using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Common.Models;
using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;

namespace EhlaklyShokran.Application.Features.WorkOrders.Queries.GetWorkOrders;

public sealed record GetWorkOrdersQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    string SortColumn = "createdAt",
    string SortDirection = "desc",
    WorkOrderState? State = null,
    Guid? CustomerId = null,
    Guid? LaborId = null,
    DateTime? StartDateFrom = null,
    DateTime? StartDateTo = null,
    DateTime? EndDateFrom = null,
    DateTime? EndDateTo = null,
    Spot? Spot = null
) : ICachedQuery<Result<PaginatedList<WorkOrderListItemDto>>>
{
    public string CacheKey =>
        $"work-orders:p={Page}:ps={PageSize}" +
        $":q={SearchTerm ?? "-"}" +
        $":sort={SortColumn}:{SortDirection}" +
        $":state={State?.ToString() ?? "-"}" +
        $":veh={CustomerId?.ToString() ?? "-"}" +
        $":lab={LaborId?.ToString() ?? "-"}" +
        $":sdfrom={StartDateFrom?.ToString("yyyyMMdd") ?? "-"}" +
        $":sdto={StartDateTo?.ToString("yyyyMMdd") ?? "-"}" +
        $":edfrom={EndDateFrom?.ToString("yyyyMMdd") ?? "-"}" +
        $":edto={EndDateTo?.ToString("yyyyMMdd") ?? "-"}" +
        $":spot={Spot?.ToString() ?? "-"}";

    public string[] Tags => ["work-order"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}