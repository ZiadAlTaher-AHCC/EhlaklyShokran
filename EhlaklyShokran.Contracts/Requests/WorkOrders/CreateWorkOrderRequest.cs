using System.ComponentModel.DataAnnotations;

using EhlaklyShokran.Contracts.Common;

namespace EhlaklyShokran.Contracts.Requests.WorkOrders;

public class CreateWorkOrderRequest
{
    [Required(ErrorMessage = "Spot is required.")]
    [Range(0, 3, ErrorMessage = "Invalid range [0, 1, 2 or 3]")]
    public Spot Spot { get; set; }

    [Required(ErrorMessage = "Customer is required.")]
    public Guid CustomerId { get; set; }

    [MinLength(1, ErrorMessage = "At least one Barber task must be selected.")]
    public List<Guid> BarberTaskIds { get; set; } = [];

    [Required(ErrorMessage = "Labor is required.")]
    public Guid LaborId { get; set; }

    [Required(ErrorMessage = "StartAt is required.")]
    public DateTimeOffset StartAtUtc { get; set; }
}