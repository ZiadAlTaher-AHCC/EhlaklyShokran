using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Labors.Dtos;
using EhlaklyShokran.Application.Features.Labors.Mappers;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Labors.Queries
{
    public class GetLaborsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetLaborsQuery, Result<List<LaborDto>>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<List<LaborDto>>> Handle(GetLaborsQuery query, CancellationToken ct)
        {
            var labors = await _context.Employees.AsNoTracking().Where(e => e.Role == Role.Labor).ToListAsync(ct);

            return labors.ToDtos();
        }
    }
}
