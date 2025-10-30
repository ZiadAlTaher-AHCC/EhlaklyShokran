using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Dtos
{
    public sealed class InvoicePdfDto
    {
        public byte[]? Content { get; init; }
        public string? FileName { get; init; }
        public string? ContentType { get; init; } = "application/pdf";
    }
}
