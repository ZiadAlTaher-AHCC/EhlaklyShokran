using EhlaklyShokran.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Infrastructure.RealTime
{
    public sealed class SignalRWorkOrderNotifier(IHubContext<WorkOrderHub> hubContext) : IWorkOrderNotifier
    {
        private readonly IHubContext<WorkOrderHub> _hubContext = hubContext;

        public Task NotifyWorkOrdersChangedAsync(CancellationToken ct = default) =>
            _hubContext.Clients.All.SendAsync("WorkOrdersChanged", cancellationToken: ct);
    }
}
