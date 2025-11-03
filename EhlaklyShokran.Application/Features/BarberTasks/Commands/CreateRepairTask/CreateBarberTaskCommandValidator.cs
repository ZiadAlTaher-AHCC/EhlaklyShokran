using FluentValidation;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.CreateBarberTask;

public sealed class CreateBarberTaskCommandValidator : AbstractValidator<CreateBarberTaskCommand>
{
    public CreateBarberTaskCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LaborCost)
            .GreaterThan(0).WithMessage("Labor cost must be greater than 0.");

        RuleFor(x => x.EstimatedDurationInMins)
            .NotNull().WithMessage("Estimated duration is required.")
            .IsInEnum();

        RuleFor(x => x.Cosmetics)
            .NotNull().WithMessage("Cosmetics list cannot be null.")
            .Must(p => p.Count > 0).WithMessage("At least one Cosmetic is required.");

        RuleForEach(x => x.Cosmetics).SetValidator(new CreateBarberTaskCosmeticCommandValidator());
    }
}