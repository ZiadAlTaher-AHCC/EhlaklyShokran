using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.BarberTasks.Enums;

using MediatR;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.UpdateBarberTask;

public sealed record UpdateBarberTaskCommand(
    Guid BarberTaskId,
    string Name,
    decimal LaborCost,
    ServiceDurationInMinutes EstimatedDurationInMins,
    List<UpdateBarberTaskCosmeticCommand> Cosmetics
) : IRequest<Result<Updated>>;