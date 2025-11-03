using EhlaklyShokran.Domain.Common.Results;

using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.UpdateWorkOrderBarberTasks;

public sealed record UpdateWorkOrderBarberTasksCommand(
    Guid WorkOrderId,
    Guid[] BarberTaskIds) : IRequest<Result<Updated>>;