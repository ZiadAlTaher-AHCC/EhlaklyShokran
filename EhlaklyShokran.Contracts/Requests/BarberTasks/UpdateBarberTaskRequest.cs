using System.ComponentModel.DataAnnotations;

using EhlaklyShokran.Contracts.Common;

namespace EhlaklyShokran.Contracts.Requests.BarberTasks;

public class UpdateBarberTaskRequest
{
    [Required(ErrorMessage = "Task name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Labor cost is required.")]
    [Range(1, 10000, ErrorMessage = "Labor cost must be between 1 and 10,000.")]
    public decimal LaborCost { get; set; }

    [Required(ErrorMessage = "Estimated duration is required.")]
    public ServiceDurationInMinutes EstimatedDurationInMins { get; set; }

    [MinLength(1, ErrorMessage = "At least one cosmatic is required.")]
    //[ValidateComplexType]
    public List<UpdateBarberTaskCosmaticRequest> Cosmatics { get; set; } = [];
}