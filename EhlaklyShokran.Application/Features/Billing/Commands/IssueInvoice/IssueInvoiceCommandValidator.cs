using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Billing.Commands.IssueInvoice
{
    public sealed class IssueInvoiceCommandValidator : AbstractValidator<IssueInvoiceCommand>
    {
        public IssueInvoiceCommandValidator()
        {
            RuleFor(request => request.WorkOrderId)
                .NotEmpty()
                .WithErrorCode("WorkOrderId_Is_Required")
                .WithMessage("WorkOrderId is required.");
        }
    }
}
