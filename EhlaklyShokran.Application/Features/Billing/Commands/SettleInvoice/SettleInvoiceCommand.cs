using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Commands.SettleInvoice
{

    public sealed record SettleInvoiceCommand(Guid InvoiceId) : IRequest<Result<Success>>;
}
