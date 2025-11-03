using EhlaklyShokran.Application.Features.Labors.Dtos;
using EhlaklyShokran.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Labors.Mappers
{
    public static class LaborMapper
    {
        public static LaborDto ToDto(this Employee employee)
        {
            return new LaborDto { LaborId = employee.Id, Name = employee.FullName };
        }

        public static List<LaborDto> ToDtos(this IEnumerable<Employee> entities)
        {
            return [.. entities.Select(l => l.ToDto())];
        }
    }
}
