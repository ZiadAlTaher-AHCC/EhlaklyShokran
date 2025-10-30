using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Common.Interfaces
{
    public interface IWorkOrderNotifier
    {
        Task NotifyWorkOrdersChangedAsync(CancellationToken ct = default);
    }
}
