using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Labors.Dtos
{
    public class LaborDto
    {
        public Guid LaborId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
