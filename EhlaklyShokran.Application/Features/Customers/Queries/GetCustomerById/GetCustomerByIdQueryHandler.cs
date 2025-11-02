using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Application.Features.Customers.Mappers;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(
    ILogger<GetCustomerByIdQueryHandler> logger,
    IApplicationDbContext context
    )
    : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery query, CancellationToken ct)
    {
        var customer = await _context.Customers.AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id == query.CustomerId, ct);

        if (customer is null)
        {
            _logger.LogWarning("Customer with id {CustomerId} was not found", query.CustomerId);

            return Error.NotFound(
                code: "Customer_NotFound",
                description: $"Customer with id '{query.CustomerId}' was not found");
        }

        return customer.ToDto();
    }
}