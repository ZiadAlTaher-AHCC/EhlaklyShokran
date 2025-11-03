using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Application.Features.BarberTasks.Mappers;
using EhlaklyShokran.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTaskById;

public class GetBarberTaskByIdQueryHandler(
    ILogger<GetBarberTaskByIdQueryHandler> logger,
    IApplicationDbContext context
    )
    : IRequestHandler<GetBarberTaskByIdQuery, Result<BarberTaskDto>>
{
    private readonly ILogger<GetBarberTaskByIdQueryHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<BarberTaskDto>> Handle(GetBarberTaskByIdQuery query, CancellationToken ct)
    {
        var barberTask = await _context.BarberTasks.AsNoTracking().Include(c => c.Cosmetics)
                                     .FirstOrDefaultAsync(c => c.Id == query.BarberTaskId, ct);

        if (barberTask is null)
        {
            _logger.LogWarning("Barber task with id {BarberTaskId} was not found", query.BarberTaskId);

            return ApplicationErrors.BarberTaskNotFound;
        }

        return barberTask.ToDto();
    }
}