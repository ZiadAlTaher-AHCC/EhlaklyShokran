using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public sealed record CreateWorkOrderCommand(
    Spot Spot,
    Guid CustomerId,
    DateTimeOffset StartAt,
    List<Guid> BarberTaskIds,
    Guid? LaborId
    )
: IRequest<Result<WorkOrderDto>>;