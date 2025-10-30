using EhlaklyShokran.Application.Features.Customers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Dtos
{
    public class InvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public Guid WorkOrderId { get; set; }
        public DateTimeOffset IssuedAtUtc { get; set; }
        public CustomerDto? Customer { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string? PaymentStatus { get; set; }

        public List<InvoiceLineItemDto> Items { get; set; } = [];
    }
}
