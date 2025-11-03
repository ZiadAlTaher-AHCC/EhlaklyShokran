using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Billing.Dtos;
using EhlaklyShokran.Application.Features.Billing.Mappers;
using EhlaklyShokran.Domain.Common.Constants;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;
using EhlaklyShokran.Domain.WorkOrders.Billing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Commands.IssueInvoice
{
    public class IssueInvoiceCommandHandler(
     ILogger<IssueInvoiceCommandHandler> logger,
     IApplicationDbContext context,
     HybridCache cache,
     TimeProvider datetime
     )
     : IRequestHandler<IssueInvoiceCommand, Result<InvoiceDto>>
    {
        private readonly ILogger<IssueInvoiceCommandHandler> _logger = logger;
        private readonly IApplicationDbContext _context = context;
        private readonly HybridCache _cache = cache;
        private readonly TimeProvider _datetime = datetime;
        public async Task<Result<InvoiceDto>> Handle(IssueInvoiceCommand command, CancellationToken ct)
        {
            var workOrder = await _context.WorkOrders
                    .Include(v => v.Customer)
                  .Include(w => w.BarberTasks)
                    .ThenInclude(rt => rt.Cosmetics)
                  .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, ct);

            if (workOrder is null)
            {
                _logger.LogWarning("Invoice issuance failed. WorkOrder {WorkOrderId} not found.", command.WorkOrderId);

                return ApplicationErrors.WorkOrderNotFound;
            }

            if (workOrder.State != WorkOrderState.Completed)
            {
                _logger.LogWarning("Invoice issuance rejected. WorkOrder {WorkOrderId} is not in completed.", command.WorkOrderId);

                return ApplicationErrors.WorkOrderMustBeCompletedForInvoicing;
            }

            Guid invoiceId = Guid.NewGuid();

            var lineItems = new List<InvoiceLineItem>();

            var lineNumber = 1;

            foreach (var (task, taskIndex) in workOrder.BarberTasks.Select((t, i) => (t, i + 1)))
            {
                var cosmeticsSummary = task.Cosmetics.Any()
                   ? string.Join(Environment.NewLine, task.Cosmetics.Select(p => $"    • {p.Name} x{p.Quantity} @ {p.Cost:C}"))
                   : "    • No parts";

                var lineDescription =
                    $"{taskIndex}: {task.Name}{Environment.NewLine}" +
                    $"  Labor = {task.LaborCost:C}{Environment.NewLine}" +
                    $"  Parts:{Environment.NewLine}{cosmeticsSummary}";

                var totalPartsCost = task.Cosmetics.Sum(p => p.Cost * p.Quantity);
                var totalTaskCost = task.LaborCost + totalPartsCost;

                var lineItemResult = InvoiceLineItem.Create(
                    invoiceId: invoiceId,
                    lineNumber: lineNumber++,
                    description: lineDescription,
                    quantity: 1,  //[FLAG] Each task is billed as a single line item we can adjust later if needed 
                    unitPrice: totalTaskCost);

                if (lineItemResult.IsError)
                {
                    return lineItemResult.Errors;
                }

                lineItems.Add(lineItemResult.Value);
            }

            var subtotal = lineItems.Sum(x => x.LineTotal);

            var taxAmount = subtotal * EhlaklyShokranConstants.TaxRate;

            var discountAmount = workOrder.Discount ?? 0m;

            var createInvoiceResult = Invoice.Create(
                id: invoiceId,
                workOrderId: workOrder.Id,
                items: lineItems,
                discountAmount: discountAmount,
                taxAmount: taxAmount,
                datetime: _datetime);

            if (createInvoiceResult.IsError)
            {
                _logger.LogWarning(
                     "Invoice creation failed for WorkOrderId: {WorkOrderId}. Errors: {@Errors}",
                     command.WorkOrderId,
                     createInvoiceResult.Errors);

                return createInvoiceResult.Errors;
            }

            var invoice = createInvoiceResult.Value;

            await _context.Invoices.AddAsync(invoice, ct);

            await _context.SaveChangesAsync(ct);

            await _cache.RemoveByTagAsync("invoice", ct);

            _logger.LogInformation("Invoice {InvoiceId} issued for WorkOrder {WorkOrderId}.", invoice.Id, workOrder.Id);

            return invoice.ToDto();

        }

    }
}
