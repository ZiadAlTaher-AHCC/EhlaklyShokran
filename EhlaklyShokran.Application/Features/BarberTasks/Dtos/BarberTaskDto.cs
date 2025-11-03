using EhlaklyShokran.Domain.BarberTasks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.BarberTasks.Dtos
{
    public class BarberTaskDto
    {
        public Guid BarberTaskId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ServiceDurationInMinutes EstimatedDurationInMins { get; set; }
        public decimal LaborCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<CosmeticDto> Cosmetics { get; set; } = [];
    }
}
