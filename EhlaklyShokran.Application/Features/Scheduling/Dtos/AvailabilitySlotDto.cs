using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Application.Features.Labors.Dtos;
using EhlaklyShokran.Domain.Workorders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Scheduling.Dtos
{
    public class AvailabilitySlotDto
    {
        public Guid? WorkOrderId { get; set; }
        public Spot Spot { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public LaborDto? Labor { get; set; }
        public bool IsOccupied { get; set; }
        public bool? IsAvailable { get; set; }
        public bool WorkOrderLocked { get; set; }
        public WorkOrderState? State { get; set; }
        public BarberTaskDto[]? BarberTasks { get; set; }
    }
}
