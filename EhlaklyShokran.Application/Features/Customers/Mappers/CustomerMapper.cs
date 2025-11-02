using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Mappers;
public static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerDto
        {
            CustomerId = entity.Id,
            Name = entity.Name!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!,
        };
    }

    public static List<CustomerDto> ToDtos(this IEnumerable<Customer> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }

 
}