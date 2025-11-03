using EhlaklyShokran.Domain.Common.Results;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.AssignLabor;

public sealed record AssignLaborCommand(Guid WorkOrderId, Guid LaborId) : IRequest<Result<Updated>>;