//using Asp.Versioning;

using EhlaklyShokran.Application.Features.BarberTasks.Commands.CreateBarberTask;
using EhlaklyShokran.Application.Features.BarberTasks.Commands.RemoveBarberTask;
using EhlaklyShokran.Application.Features.BarberTasks.Commands.UpdateBarberTask;
using EhlaklyShokran.Application.Features.BarberTasks.Dtos;
using EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTaskById;
using EhlaklyShokran.Application.Features.BarberTasks.Queries.GetBarberTasks;
using EhlaklyShokran.Contracts.Requests.BarberTasks;
using EhlaklyShokran.Domain.BarberTasks.Cosmetics;
using EhlaklyShokran.Domain.BarberTasks.Enums;
//using EhlaklyShokran.Contracts.Requests.BarberTasks;
using EhlaklyShokran.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace EhlaklyShokran.Api.Controllers;

[Route("api/v{version:apiVersion}/barber-tasks")]
[ApiVersion("1.0")]
[Authorize]
public sealed class BarberTasksController(ISender sender) : ApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(List<BarberTaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves all Barber tasks.")]
    [EndpointDescription("Returns a list of all barber tasks available in the system.")]
    [EndpointName("GetBarberTasks")]
    [MapToApiVersion("1.0")]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await sender.Send(new GetBarberTasksQuery(), ct);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("{barberTaskId:guid}", Name = nameof(GetById))]
    [ProducesResponseType(typeof(BarberTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves a barber task by ID.")]
    [EndpointDescription("Returns detailed information for the specified Barber task if it exists.")]
    [EndpointName("GetBarberTaskById")]
    [MapToApiVersion("1.0")]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetById(Guid barberTaskId, CancellationToken ct)
    {
        var result = await sender.Send(new GetBarberTaskByIdQuery(barberTaskId), ct);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Manager))]
    //[ProducesResponseType(typeof(BarberTaskDto), StatusCodes.Status201Created)]
    //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new Barber task.")]
    [EndpointDescription("Creates a Barber task and optionally includes Cosmatics.")]
    [EndpointName("CreateBarberTask")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Create(/*[FromBody] CreateBarberTaskRequest request,*/ CancellationToken ct)
    {
//        var Cosmatics = request.Cosmatics
//        .ConvertAll(p => new CreateBarberTaskCosmeticCommand(p.Name, p.Cost, p.Quantity))
//;

//        var command = new CreateBarberTaskCommand(
//            request.Name,
//            request.LaborCost,
//            request.EstimatedDurationInMins is not null ? (ServiceDurationInMinutes)request.EstimatedDurationInMins : null,
//            Cosmatics);

//        var result = await sender.Send(command, ct);

//        return result.Match(
//            response => CreatedAtAction(nameof(GetById), new { BarberTaskId = response.BarberTaskId }, response),
//            Problem);
        return Ok();

    }

    [HttpPut("{BarberTaskId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    //[ProducesResponseType(typeof(BarberTaskDto), StatusCodes.Status204NoContent)]
    //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Updates an existing Barber task.")]
    [EndpointDescription("Updates a Barber task and its associated Cosmatics.")]
    [EndpointName("UpdateBarberTask")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Update(Guid BarberTaskId, [FromBody] UpdateBarberTaskRequest request, CancellationToken ct)
    {
        //        var cosmatics = request.Cosmatics
        //            .ConvertAll(p => new UpdateBarberTaskCosmeticCommand(p.CosmaticId, p.Name, p.Cost, p.Quantity))
        //;

        //        var command = new UpdateBarberTaskCommand(
        //            BarberTaskId,
        //            request.Name,
        //            request.LaborCost,
        //            (ServiceDurationInMinutes)request.EstimatedDurationInMins,
        //            cosmatics);

        //        var result = await sender.Send(command, ct);

        //        return result.Match(
        //            response => Ok(response),
        //            Problem);
        return Ok();
    }

    [HttpDelete("{BarberTaskId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Removes a Barber task.")]
    [EndpointDescription("Deletes the specified Barber task from the system.")]
    [EndpointName("RemoveBarberTask")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Delete(Guid BarberTaskId, CancellationToken ct)
    {
        var result = await sender.Send(new RemoveBarberTaskCommand(BarberTaskId), ct);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}