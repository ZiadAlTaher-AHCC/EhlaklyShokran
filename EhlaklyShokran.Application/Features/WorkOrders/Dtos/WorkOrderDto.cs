using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Application.Features.Labors.Dtos;
using EhlaklyShokran.Domain.Workorders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.WorkOrders.Dtos
{
    public class WorkOrderDto
    {
        public Guid WorkOrderId { get; set; }
        public Guid? InvoiceId { get; set; }
        public Spot Spot { get; set; }
        public DateTimeOffset StartAtUtc { get; set; }
        public DateTimeOffset EndAtUtc { get; set; }
        public List<BarberTaskDto> BarberTasks { get; set; } = [];
        public LaborDto? Labor { get; set; }
        public CustomerDto? Customer { get; set; }
        public WorkOrderState State { get; set; }
        public decimal TotalPartCost { get; set; }
        public decimal TotalLaborCost { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalDurationInMins { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
