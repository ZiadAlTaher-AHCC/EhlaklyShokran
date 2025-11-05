using FluentValidation;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.UpdateBarberTask;

public class UpdateBarberTaskCommandValidator : AbstractValidator<UpdateBarberTaskCommand>
{
    public UpdateBarberTaskCommandValidator()
    {
        RuleFor(x => x.BarberTaskId)
            .NotEmpty().WithMessage("Barber task ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Task name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LaborCost)
            .InclusiveBetween(1, 10_000)
            .WithMessage("Labor cost must be between 1 and 10,000.");

        RuleFor(x => x.EstimatedDurationInMins)
            .IsInEnum()
            .WithMessage("Invalid duration selected.");

        RuleFor(x => x.Cosmetics)
            .NotNull()
            .Must(p => p.Count > 0)
            .WithMessage("At least one Cosmetic is required.");

        RuleForEach(x => x.Cosmetics)
            .SetValidator(new UpdateBarberTaskCosmeticCommandValidator());
    }
}