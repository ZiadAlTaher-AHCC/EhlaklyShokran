using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.WorkOrders.Events;
using MediatR;

namespace EhlaklyShokran.Application.Features.WorkOrders.EventHandlers;

public sealed class WorkOrderCollectionModifiedEventHandler(IWorkOrderNotifier notifier)
        : INotificationHandler<WorkOrderCollectionModified>
{
    private readonly IWorkOrderNotifier _notifier = notifier;

    public Task Handle(WorkOrderCollectionModified notification, CancellationToken ct) =>
        _notifier.NotifyWorkOrdersChangedAsync(ct);
}