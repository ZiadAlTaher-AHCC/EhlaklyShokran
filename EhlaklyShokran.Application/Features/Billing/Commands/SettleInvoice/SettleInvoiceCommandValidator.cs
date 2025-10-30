using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Commands.SettleInvoice
{
    public sealed class SettleInvoiceCommandValidator : AbstractValidator<SettleInvoiceCommand>
    {
        public SettleInvoiceCommandValidator()
        {
            RuleFor(request => request.InvoiceId)
                .NotEmpty()
                .WithErrorCode("InvoiceId_Is_Required")
                .WithMessage("InvoiceId is required.");
        }
    }
}
