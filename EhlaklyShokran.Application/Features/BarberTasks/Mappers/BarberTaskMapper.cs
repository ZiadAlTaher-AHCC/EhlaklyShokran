using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Domain.BarberTasks;
using EhlaklyShokran.Domain.BarberTasks.Cosmetics;

namespace EhlaklyShokran.Application.Features.BarberTasks.Mappers;

public static class BarberTaskMapper
{
    public static BarberTaskDto ToDto(this BarberTask entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new BarberTaskDto
        {
            BarberTaskId = entity.Id,
            Name = entity.Name!,
            LaborCost = entity.LaborCost,
            TotalCost = entity.TotalCost,
            EstimatedDurationInMins = entity.EstimatedDurationInMins,
            Cosmetics = entity.Cosmetics.ToList().ConvertAll(ToDto)
        };
    }

    public static List<BarberTaskDto> ToDtos(this IEnumerable<BarberTask> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }

    public static CosmeticDto ToDto(this Cosmetic entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CosmeticDto
        {
            CosmeticId = entity.Id,
            Name = entity.Name!,
            Cost = entity.Cost,
            Quantity = entity.Quantity
        };
    }

    public static List<CosmeticDto> ToDtos(this IEnumerable<Cosmetic> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}