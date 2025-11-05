using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Billing.Dtos;
using EhlaklyShokran.Application.Features.Billing.Mappers;
using EhlaklyShokran.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.Billing.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryHandler(
    ILogger<GetInvoiceByIdQueryHandler> logger,
    IApplicationDbContext context
    )
    : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDto>>
{
    public async Task<Result<InvoiceDto>> Handle(GetInvoiceByIdQuery query, CancellationToken ct)
    {
        var invoice = await context.Invoices.AsNoTracking()
            .Include(i => i.LineItems)
            .Include(i => i.WorkOrder!)
                    .ThenInclude(v => v.Customer)
            .FirstOrDefaultAsync(i => i.Id == query.InvoiceId, ct);

        if (invoice is null)
        {
            logger.LogWarning("Invoice not found. InvoiceId: {InvoiceId}", query.InvoiceId);
            return Error.NotFound("Invoice not found.");
        }

        return invoice.ToDto();
    }
}