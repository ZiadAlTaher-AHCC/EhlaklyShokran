using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Application.Features.Customers.Mappers;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(IApplicationDbContext context
    )
    : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery query, CancellationToken ct)
    {
        var customers = await _context.Customers.AsNoTracking().ToListAsync(ct);

        return customers.ToDtos();
    }
}