using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Billing.Dtos;
using EhlaklyShokran.Domain.Common.Results;

namespace EhlaklyShokran.Application.Features.Billing.Queries.GetInvoiceById;

public sealed record GetInvoiceByIdQuery(Guid InvoiceId) : ICachedQuery<Result<InvoiceDto>>
{
    public string CacheKey => $"invoice_{InvoiceId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["invoice"];
}