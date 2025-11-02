using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Customers.Commands.RemoveCustomer;
public sealed record RemoveCustomerCommand(Guid CustomerId)
    : IRequest<Result<Deleted>>;