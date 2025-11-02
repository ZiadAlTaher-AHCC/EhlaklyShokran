using EhlaklyShokran.Application.Features.Customers.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string PhoneNumber,
    string Email

) : IRequest<Result<CustomerDto>>;
