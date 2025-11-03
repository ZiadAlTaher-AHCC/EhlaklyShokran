using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.BarberTasks.Enums;

using MediatR;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.CreateBarberTask;

public sealed record CreateBarberTaskCommand(
    string? Name,
    decimal LaborCost,
    ServiceDurationInMinutes? EstimatedDurationInMins,
    List<CreateBarberTaskCosmeticCommand> Cosmetics
) : IRequest<Result<BarberTaskDto>>;