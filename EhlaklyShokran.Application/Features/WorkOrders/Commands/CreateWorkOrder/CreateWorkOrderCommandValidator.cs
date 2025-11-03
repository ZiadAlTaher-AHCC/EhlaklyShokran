using FluentValidation;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public sealed class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
{
    public CreateWorkOrderCommandValidator()
    {
        RuleFor(request => request.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");

        RuleFor(request => request.StartAt)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("StartAt must be in the future.");

        RuleFor(request => request.BarberTaskIds)
            .NotEmpty()
            .WithMessage("At least one Barber task must be selected");

        RuleFor(request => request.LaborId)
            .Must(laborId => laborId is null || laborId != Guid.Empty)
            .WithMessage("If provided, LaborId must not be empty.");

        RuleFor(x => x.Spot)
          .IsInEnum()
          .WithErrorCode("Spot_Invalid")
          .WithMessage("Spot must be a valid Spot value. [A, B, C, D]");
    }
}