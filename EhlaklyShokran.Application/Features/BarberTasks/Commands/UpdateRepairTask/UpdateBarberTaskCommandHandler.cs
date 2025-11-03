using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.BarberTasks.Cosmetics;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.UpdateBarberTask;

public class UpdateBarberTaskCommandHandler(
    ILogger<UpdateBarberTaskCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache
    )
    : IRequestHandler<UpdateBarberTaskCommand, Result<Updated>>
{
    private readonly ILogger<UpdateBarberTaskCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<Updated>> Handle(UpdateBarberTaskCommand command, CancellationToken ct)
    {
        var BarberTask = await _context.BarberTasks
            .Include(rt => rt.Cosmetics)
            .FirstOrDefaultAsync(rt => rt.Id == command.BarberTaskId, ct);

        if (BarberTask is null)
        {
            _logger.LogWarning("BarberTask {BarberTaskId} not found for update.", command.BarberTaskId);

            return ApplicationErrors.BarberTaskNotFound;
        }

        var validatedCosmetics = new List<Cosmetic>();

        foreach (var p in command.Cosmetics)
        {
            var cosmeticId = p.CosmeticId ?? Guid.NewGuid();

            var cosmeticResult = Cosmetic.Create(cosmeticId, p.Name, p.Cost, p.Quantity);

            if (cosmeticResult.IsError)
            {
                return cosmeticResult.Errors;
            }

            validatedCosmetics.Add(cosmeticResult.Value);
        }

        var updateBarberTaskResult = BarberTask.Update(command.Name, command.LaborCost, command.EstimatedDurationInMins);

        if (updateBarberTaskResult.IsError)
        {
            return updateBarberTaskResult.Errors;
        }

        var upsertCosmeticsResult = BarberTask.UpsertCosmetics(validatedCosmetics);

        if (upsertCosmeticsResult.IsError)
        {
            return upsertCosmeticsResult.Errors;
        }

        await _context.SaveChangesAsync(ct); // The database operation was expected to affect 1 row(s), but actually affected 0 row(s);

        await _cache.RemoveByTagAsync("Barber-task", ct);

        return Result.Updated;
    }
}