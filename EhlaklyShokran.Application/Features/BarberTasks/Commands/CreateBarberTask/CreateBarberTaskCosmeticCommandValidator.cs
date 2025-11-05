using FluentValidation;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.CreateBarberTask;

public sealed class CreateBarberTaskCosmeticCommandValidator : AbstractValidator<CreateBarberTaskCosmeticCommand>
{
    public CreateBarberTaskCosmeticCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Cosmetic name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Cost)
            .GreaterThan(0).WithMessage("Cosmetic cost must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1.");
    }
}