using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Application.Features.BarberTasks.Mappers;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.BarberTasks;
using EhlaklyShokran.Domain.BarberTasks.Cosmetics;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.BarberTasks.Commands.CreateBarberTask;

public class CreateBarberTaskCommandHandler(
    ILogger<CreateBarberTaskCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache
    )
    : IRequestHandler<CreateBarberTaskCommand, Result<BarberTaskDto>>
{
    private readonly ILogger<CreateBarberTaskCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<BarberTaskDto>> Handle(CreateBarberTaskCommand command, CancellationToken ct)
    {
        var nameExists = await _context.BarberTasks
           .AnyAsync(p => EF.Functions.Like(p.Name, command.Name), ct);

        if (nameExists)
        {
            _logger.LogWarning("Duplicate cosmetic name '{CosmeticName}'.", command.Name);

            return BarberTaskErrors.DuplicateName;
        }

        List<Cosmetic> cosmetics = [];

        foreach (var p in command.Cosmetics)
        {
            var cosmeticResult = Cosmetic.Create(Guid.NewGuid(), p.Name, p.Cost, p.Quantity);

            if (cosmeticResult.IsError)
            {
                return cosmeticResult.Errors;
            }

            cosmetics.Add(cosmeticResult.Value);
        }

        var createBarberTaskResult = BarberTask.Create(
                    id: Guid.NewGuid(),
                    name: command.Name!,
                    laborCost: command.LaborCost,
                    estimatedDurationInMins: command.EstimatedDurationInMins!.Value,
                    Cosmetics: cosmetics);

        if (createBarberTaskResult.IsError)
        {
            return createBarberTaskResult.Errors;
        }

        var barberTask = createBarberTaskResult.Value;

        _context.BarberTasks.Add(barberTask);

        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync("Barber-task", ct);

        return barberTask.ToDto();
    }
}