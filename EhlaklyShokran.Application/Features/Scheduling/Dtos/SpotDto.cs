using EhlaklyShokran.Domain.Workorders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Scheduling.Dtos
{
    public class SpotDto
    {
        public Spot Spot { get; set; }
        public List<AvailabilitySlotDto> Slots { get; set; } = [];
    }
}
