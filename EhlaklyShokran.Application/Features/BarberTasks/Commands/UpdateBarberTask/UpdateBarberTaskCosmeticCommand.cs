namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.UpdateBarberTask;

public sealed record UpdateBarberTaskCosmeticCommand(
    Guid? CosmeticId,
    string Name,
    decimal Cost,
    int Quantity
);