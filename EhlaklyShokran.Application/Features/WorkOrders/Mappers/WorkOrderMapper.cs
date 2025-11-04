using EhlaklyShokran.Application.Features.BarberTasks.Mappers;
using EhlaklyShokran.Application.Features.Customers.Mappers;
using EhlaklyShokran.Application.Features.Labors.Dtos;
using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Domain.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.WorkOrders.Mappers
{

    public static class WorkOrderMapper
    {
        public static WorkOrderDto ToDto(this WorkOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WorkOrderDto
            {
                WorkOrderId = entity.Id,
                Spot = entity.Spot,
                StartAtUtc = entity.StartAtUtc,
                EndAtUtc = entity.EndAtUtc,
                Labor = entity.Labor is null ? null : new LaborDto
                {
                    LaborId = entity.LaborId,
                    Name = $"{entity.Labor.FirstName} {entity.Labor.LastName}"
                },
                BarberTasks = entity.BarberTasks.ToDtos(),
                Customer = entity.Customer is null ? null : entity.Customer.ToDto(),
                State = entity.State,
                TotalCosmeticCost = entity.BarberTasks.SelectMany(t => t.Cosmetics).Sum(p => p.Cost * p.Quantity),
                TotalLaborCost = entity.BarberTasks.Sum(p => p.LaborCost),
                TotalCost = entity.BarberTasks.Sum(rt => rt.TotalCost),
                TotalDurationInMins = entity.BarberTasks.Sum(rt => (int)rt.EstimatedDurationInMins),
                InvoiceId = entity.Invoice?.Id,
                CreatedAt = entity.CreatedAtUtc
            };
        }

        public static List<WorkOrderDto> ToDtos(this IEnumerable<WorkOrder> entities)
        {
            return [.. entities.Select(e => e.ToDto())];
        }

        public static WorkOrderListItemDto ToListItemDto(this WorkOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WorkOrderListItemDto
            {
                WorkOrderId = entity.Id,
                Spot = entity.Spot,
                StartAtUtc = entity.StartAtUtc,
                EndAtUtc = entity.EndAtUtc,
                Customer = entity.Customer!.ToDto(),
                Labor = entity.Labor is null ? null :
                    $"{entity.Labor.FirstName} {entity.Labor.LastName}",
                State = entity.State,
                BarberTasks = entity.BarberTasks.Select(rt => rt.Name).ToList()
            };
        }
    }

}
