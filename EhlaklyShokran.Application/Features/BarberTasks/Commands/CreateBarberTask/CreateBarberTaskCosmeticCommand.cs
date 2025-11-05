using EhlaklyShokran.Domain.Common.Results;

using MediatR;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.CreateBarberTask;

public sealed record CreateBarberTaskCosmeticCommand(
    string Name,
    decimal Cost,
    int Quantity
) : IRequest<Result<Success>>;