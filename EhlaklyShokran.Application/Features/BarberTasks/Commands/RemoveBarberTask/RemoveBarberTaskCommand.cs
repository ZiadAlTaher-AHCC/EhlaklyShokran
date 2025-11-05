using FluentValidation;

using EhlaklyShokran.Domain.Common.Results;

using MediatR;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.RemoveBarberTask;

public sealed record RemoveBarberTaskCommand(Guid BarberTaskId)
    : IRequest<Result<Deleted>>;

public class RemoveBarberTaskCommandValidator : AbstractValidator<RemoveBarberTaskCommand>
{
    public RemoveBarberTaskCommandValidator()
    {
        RuleFor(x => x.BarberTaskId)
            .NotEmpty().WithMessage("Barber task Id is required.");
    }
}