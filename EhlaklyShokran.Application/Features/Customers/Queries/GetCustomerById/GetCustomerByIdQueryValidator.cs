using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Queries.GetCustomerById;
public sealed class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(request => request.CustomerId)
            .NotEmpty()
            .WithErrorCode("CustomerId_Is_Required")
            .WithMessage("CustomerId is required.");
    }
}