namespace EhlaklyShokran.Contracts.Requests.WorkOrders;

public class ModifyBarberTaskRequest
{
    public Guid[] BarberTaskIds { get; set; } = [];
}