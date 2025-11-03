using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.RelocateWorkOrder;

public sealed record RelocateWorkOrderCommand(
    Guid WorkOrderId,
    DateTimeOffset NewStartAt,
    Spot NewSpot) : IRequest<Result<Updated>>;