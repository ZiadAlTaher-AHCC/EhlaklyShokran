using EhlaklyShokran.Application.Common.Errors;
using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.WorkOrders.Dtos;
using EhlaklyShokran.Application.Features.WorkOrders.Mappers;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders;
using EhlaklyShokran.Domain.WorkOrders;
using EhlaklyShokran.Domain.WorkOrders.Events;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EhlaklyShokran.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderCommandHandler(
    ILogger<CreateWorkOrderCommandHandler> logger,
    IApplicationDbContext context,
    HybridCache cache,
    IWorkOrderPolicy workOrderValidator
    )
    : IRequestHandler<CreateWorkOrderCommand, Result<WorkOrderDto>>
{
    private readonly ILogger<CreateWorkOrderCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly HybridCache _cache = cache;
    private readonly IWorkOrderPolicy _workOrderPolicy = workOrderValidator;

    public async Task<Result<WorkOrderDto>> Handle(CreateWorkOrderCommand command, CancellationToken ct)
    {
        var BarberTasks = await _context.BarberTasks
            .Where(t => command.BarberTaskIds.Contains(t.Id))
            .ToListAsync(ct);

        if (BarberTasks.Count != command.BarberTaskIds.Count)
        {
            var missingIds = command.BarberTaskIds.Except(BarberTasks.Select(t => t.Id)).ToArray();

            _logger.LogError("Some BarberTaskIds not found: {MissingIds}", string.Join(", ", missingIds));

            return ApplicationErrors.BarberTaskNotFound;
        }

        var totalEstimatedDuration = TimeSpan.FromMinutes(BarberTasks.Sum(r => (int)r.EstimatedDurationInMins));
        var endAt = command.StartAt.Add(totalEstimatedDuration);

        if (_workOrderPolicy.IsOutsideOperatingHours(command.StartAt, totalEstimatedDuration))
        {
            _logger.LogError("The WorkOrder time ({StartAt} ? {EndAt}) is outside of store operating hours.", command.StartAt, endAt);

            return ApplicationErrors.WorkOrderOutsideOperatingHour(command.StartAt, endAt);
        }

        var checkMinRequirementResult = _workOrderPolicy.ValidateMinimumRequirement(command.StartAt, endAt);

        if (checkMinRequirementResult.IsError)
        {
            _logger.LogError("WorkOrder duration is shorter than the configured minimum.");

            return checkMinRequirementResult.Errors;
        }

        var checkSpotAvailabilityResult = await _workOrderPolicy.CheckSpotAvailabilityAsync(
            command.Spot,
            command.StartAt,
            endAt,
            excludeWorkOrderId: null,
            ct);

        if (checkSpotAvailabilityResult.IsError)
        {
            _logger.LogError("Spot: {Spot} is not available.", command.Spot.ToString());
            return checkSpotAvailabilityResult.Errors;
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(v => v.Id == command.CustomerId, cancellationToken: ct);

        if (customer is null)
        {
            _logger.LogError("Customer with Id '{CustomerId}' does not exist.", command.CustomerId);

            return ApplicationErrors.CustomerNotFound;
        }

        var labor = await _context.Employees.FindAsync([command.LaborId], ct);

        if (labor is null)
        {
            _logger.LogError("Invalid LaborId: {LaborId}", command.LaborId.ToString());
            return ApplicationErrors.LaborNotFound;
        }

        var hasCustomerConflict = await _context.WorkOrders
            .AnyAsync(
                a =>
                a.CustomerId == command.CustomerId &&
                a.StartAtUtc.Date == command.StartAt.Date &&
                a.StartAtUtc < endAt &&
                a.EndAtUtc > command.StartAt,
                ct);

        if (hasCustomerConflict)
        {
            _logger.LogError("Customer with Id '{CustomerId}' already has an overlapping WorkOrder.", command.CustomerId);
            return Error.Conflict(
                code: "Customer_Overlapping_WorkOrders",
                description: "The customer already has an overlapping WorkOrder.");
        }

        var isLaborOccupied = await _context.WorkOrders
            .AnyAsync(
                a =>
                a.LaborId == command.LaborId &&
                a.StartAtUtc < endAt &&
                a.EndAtUtc > command.StartAt,
                ct);

        if (isLaborOccupied)
        {
            _logger.LogError("Labor with Id '{LaborId}' is already occupied during the requested time.", command.LaborId);
            return Error.Conflict(
                code: "Labor_Occupied",
                description: "Labor is already occupied during the requested time.");
        }

        var createWorkOrderResult = WorkOrder.Create(
            Guid.NewGuid(),
            command.CustomerId,
            command.StartAt,
            endAt,
            command.LaborId!.Value,
            command.Spot,
            BarberTasks);

        if (createWorkOrderResult.IsError)
        {
            _logger.LogError("Failed to create WorkOrder: {Error}", createWorkOrderResult.TopError.Description);

            return createWorkOrderResult.Errors;
        }

        var workOrder = createWorkOrderResult.Value;

        _context.WorkOrders.Add(workOrder);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await _context.SaveChangesAsync(ct);

        workOrder.Customer = customer;
        workOrder.Labor = labor;

        _logger.LogInformation("WorkOrder with Id '{WorkOrderId}' created successfully.", workOrder.Id);

        await _cache.RemoveByTagAsync("work-order", ct);

        return workOrder.ToDto();
    }
}