using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Contracts.Requests.WorkOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EhlaklyShokran.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecastZiad")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //[HttpPost("{workOrderId:guid}")]
        //[Authorize(Policy = "ManagerOnly")]
        ////  (typeof(WorkOrderDto), StatusCodes.Status201Created)]
        ////[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //[EndpointSummary("Creates a new work order.")]
        //[EndpointDescription("Creates a new work order for a Customer, specifying labor, tasks, and other required information.")]
        //[EndpointName("CreateWorkOrder")]
        //[MapToApiVersion("1.0")]
        //public IEnumerable<WeatherForecast> GetTest(Guid workOrderId, [FromBody] CreateWorkOrderRequest request, CancellationToken ct)
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}


        //[HttpPost]
        //[Authorize(Policy = "ManagerOnly")]
        //[ProducesResponseType(typeof(WorkOrderDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //[EndpointSummary("Creates a new work order.")]
        //[EndpointDescription("Creates a new work order for a Customer, specifying labor, tasks, and other required information.")]
        //[EndpointName("CreateWorkOrder")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> Create([FromBody] CreateWorkOrderRequest request, CancellationToken ct)
        //{
        //    var result = await sender.Send(
        //        new CreateWorkOrderCommand(
        //        (Spot)(int)request.Spot,
        //        request.CustomerId,
        //        request.StartAtUtc,
        //        request.BarberTaskIds,
        //        request.LaborId),
        //        ct);

        //    return result.Match(
        //        response => CreatedAtRoute(
        //            routeName: "GetWorkOrderById",
        //            routeValues: new { version = "1.0", workOrderId = response.WorkOrderId },
        //            value: response),
        //        Problem);
        //}
    }
}
