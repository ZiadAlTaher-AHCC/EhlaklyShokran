using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.BarberTasks.Dtos
{
    public class CosmeticDto
    {

        public Guid CosmeticId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
    }
}
