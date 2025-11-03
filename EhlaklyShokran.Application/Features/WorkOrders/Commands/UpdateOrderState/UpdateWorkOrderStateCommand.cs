using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.UpdateOrderState;

public sealed record UpdateWorkOrderStateCommand(
    Guid WorkOrderId,
    WorkOrderState State) : IRequest<Result<Updated>>;