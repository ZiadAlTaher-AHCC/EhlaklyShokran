using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Infrastructure.RealTime
{
    public sealed class WorkOrderHub : Hub
    {
        public const string HubUrl = "/hubs/workorders";
    }
}
