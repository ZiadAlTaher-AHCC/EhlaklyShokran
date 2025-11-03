using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Identity.Queries.GenerateTokens
{
    public sealed class GenerateTokenQueryValidator : AbstractValidator<GenerateTokenQuery>
    {
        public GenerateTokenQueryValidator()
        {
            RuleFor(request => request.Email)
                .NotNull().NotEmpty()
                .WithErrorCode("Email_Null_Or_Empty")
                .WithMessage("Email cannot be null or empty");

            RuleFor(request => request.Password)
                .NotNull().NotEmpty()
                .WithErrorCode("Password_Null_Or_Empty")
                .WithMessage("Password cannot be null or empty.");
        }
    }
}
