using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : ICachedQuery<Result<CustomerDto>>
{
    public string CacheKey => $"customer_{CustomerId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["customer"];
}