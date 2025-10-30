using EhlaklyShokran.Application.Features.Billing.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Commands.IssueInvoice
{
    public sealed record IssueInvoiceCommand(Guid WorkOrderId) : IRequest<Result<InvoiceDto>>;
}
