using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public sealed record CreateWorkOrderCommand(
    Spot Spot,
    DateTimeOffset StartAt,
    List<Guid> BarberTaskIds,
    Guid? LaborId,
    Guid CustomerId
    )
: IRequest<Result<WorkOrderDto>>;