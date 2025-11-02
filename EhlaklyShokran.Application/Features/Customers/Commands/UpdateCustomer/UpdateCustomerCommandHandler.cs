using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(
    ILogger<UpdateCustomerCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache
    )
    : IRequestHandler<UpdateCustomerCommand, Result<Updated>>
{
    private readonly ILogger<UpdateCustomerCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<Updated>> Handle(UpdateCustomerCommand command, CancellationToken ct)
    {
        var customer = await _context.Customers
             .FirstOrDefaultAsync(rt => rt.Id == command.CustomerId, ct);

        if (customer is null)
        {
            _logger.LogWarning("Customer {CustomerId} not found for update.", command.CustomerId);

            return ApplicationErrors.CustomerNotFound;
        }


        var updateCustomerResult = customer.Update(command.Name, command.Email, command.PhoneNumber);

        if (updateCustomerResult.IsError)
        {
            return updateCustomerResult.Errors;
        }

        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync("customer", ct);

        return Result.Updated;
    }
}