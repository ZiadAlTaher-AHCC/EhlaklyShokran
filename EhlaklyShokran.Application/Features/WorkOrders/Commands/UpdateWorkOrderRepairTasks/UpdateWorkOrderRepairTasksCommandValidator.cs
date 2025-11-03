using FluentValidation;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.UpdateWorkOrderBarberTasks;

public sealed class UpdateWorkOrderBarberTasksCommandValidator : AbstractValidator<UpdateWorkOrderBarberTasksCommand>
{
    public UpdateWorkOrderBarberTasksCommandValidator()
    {
        RuleFor(x => x.WorkOrderId)
           .NotEmpty()
           .WithErrorCode("WorkOrderId_Required")
           .WithMessage("WorkOrderId is required.");

        RuleFor(x => x.BarberTaskIds)
          .NotEmpty()
          .WithErrorCode("BarberTasks_Required")
          .WithMessage("At least one Barber task must be provided.");
    }
}