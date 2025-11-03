using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Domain.Common.Results;

namespace EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTasks;

public sealed record GetBarberTasksQuery() : ICachedQuery<Result<List<BarberTaskDto>>>
{
    public string CacheKey => "barber-tasks";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["barber-tasks"];
}