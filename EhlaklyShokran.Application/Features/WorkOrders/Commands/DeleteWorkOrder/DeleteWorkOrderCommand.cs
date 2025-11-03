using EhlaklyShokran.Domain.Common.Results;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public sealed record DeleteWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<Deleted>>;