using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Commands.UpdateCustomer;
public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string Name,
    string PhoneNumber,
    string Email

) : IRequest<Result<Updated>>;