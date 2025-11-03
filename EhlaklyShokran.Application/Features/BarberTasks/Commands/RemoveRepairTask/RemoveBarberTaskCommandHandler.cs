using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.BarberTasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.RemoveBarberTask;

public class RemoveBarberTaskCommandHandler(
    ILogger<RemoveBarberTaskCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache
    )
    : IRequestHandler<RemoveBarberTaskCommand, Result<Deleted>>
{
    private readonly ILogger<RemoveBarberTaskCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<Deleted>> Handle(RemoveBarberTaskCommand command, CancellationToken ct)
    {
        var BarberTask = await _context.BarberTasks
            .FindAsync([command.BarberTaskId], ct);

        if (BarberTask is null)
        {
            _logger.LogWarning("BarberTask {BarberTaskId} not found for deletion.", command.BarberTaskId);
            return ApplicationErrors.BarberTaskNotFound;
        }

        var isInUse = await _context.WorkOrders.AsNoTracking()
            .SelectMany(x => x.BarberTasks)
            .AnyAsync(rt => rt.Id == command.BarberTaskId, ct);

        if (isInUse)
        {
            _logger.LogWarning("BarberTask {BarberTaskId} cannot be deleted — in use by work orders.", command.BarberTaskId);

            return BarberTaskErrors.InUse;
        }

        _context.BarberTasks.Remove(BarberTask);
        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync("Barber-task", ct);

        _logger.LogInformation("BarberTask {BarberTaskId} deleted successfully.", command.BarberTaskId);

        return Result.Deleted;
    }
}