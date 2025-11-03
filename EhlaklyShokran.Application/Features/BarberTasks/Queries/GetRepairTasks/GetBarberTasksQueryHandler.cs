using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Application.Features.BarberTasks.Mappers;
using EhlaklyShokran.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTasks;

public class GetBarberTasksQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBarberTasksQuery, Result<List<BarberTaskDto>>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<List<BarberTaskDto>>> Handle(GetBarberTasksQuery query, CancellationToken ct)
    {
        var barberTasks = await _context.BarberTasks.Include(rt => rt.Cosmetics).AsNoTracking().ToListAsync(ct);

        return barberTasks.ToDtos();
    }
}