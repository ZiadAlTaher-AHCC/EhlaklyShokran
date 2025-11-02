using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Queries.GetCustomers;
public sealed record GetCustomersQuery : ICachedQuery<Result<List<CustomerDto>>>
{
    public string CacheKey => "customers";
    public string[] Tags => ["customer"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}