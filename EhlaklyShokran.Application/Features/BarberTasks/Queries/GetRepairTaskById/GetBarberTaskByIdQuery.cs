using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Domain.Common.Results;

namespace EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTaskById;

public sealed record GetBarberTaskByIdQuery(Guid BarberTaskId) : ICachedQuery<Result<BarberTaskDto>>
{
    public string CacheKey => $"barber-task_{BarberTaskId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["barber-task"];
}