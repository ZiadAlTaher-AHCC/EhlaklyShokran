using EhlaklyShokran.Contracts.Common;

namespace EhlaklyShokran.Contracts.Requests.WorkOrders;

public class UpdateWorkOrderStateRequest
{
    public WorkOrderState State { get; set; }
}