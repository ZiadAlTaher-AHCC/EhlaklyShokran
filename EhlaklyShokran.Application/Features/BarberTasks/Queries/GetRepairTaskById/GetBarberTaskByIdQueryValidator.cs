using FluentValidation;

namespace EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTaskById;

public sealed class GetBarberTaskByIdQueryValidator : AbstractValidator<GetBarberTaskByIdQuery>
{
    public GetBarberTaskByIdQueryValidator()
    {
        RuleFor(request => request.BarberTaskId)
            .NotEmpty()
            .WithErrorCode("BarberTaskId_Is_Required")
            .WithMessage("CustomerId is required.");
    }
}