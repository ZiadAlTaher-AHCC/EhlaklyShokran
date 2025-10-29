using EhlaklyShokran.Domain.BarberServices;
using EhlaklyShokran.Domain.Common;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders;
using EhlaklyShokran.Domain.Workorders.Enums;
using EhlaklyShokran.Domain.WorkOrders.Billing;
using EhlaklyShokran.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.WorkOrders
{

    public sealed class WorkOrder : AuditableEntity
    {
        public Guid VehicleId { get; }
        public DateTimeOffset StartAtUtc { get; private set; }
        public DateTimeOffset EndAtUtc { get; private set; }
        public Guid LaborId { get; private set; }
        public Spot Spot { get; private set; }
        public WorkOrderState State { get; private set; }
        public Employee? Labor { get; set; }
        public Invoice? Invoice { get; set; }
        public decimal? Discount { get; private set; }
        public decimal? Tax { get; private set; }
        public decimal? TotalPartsCost => _barberService.SelectMany(rt => rt.Cosmetics).Sum(p => p.Cost);
        public decimal? TotalLaborCost => _barberService.Sum(rt => rt.LaborCost);
        public decimal? Total => (TotalPartsCost ?? 0) + (TotalLaborCost ?? 0);

        private readonly List<BarberService> _barberService = [];
        public IEnumerable<BarberService> BarberServices => _barberService.AsReadOnly();

        private WorkOrder()
        { }

        private WorkOrder(Guid id, Guid vehicleId, DateTimeOffset startAt, DateTimeOffset endAt, Guid laborId, Spot spot, WorkOrderState state, List<BarberService> barberServices)
            : base(id)
        {
            VehicleId = vehicleId;
            StartAtUtc = startAt;
            EndAtUtc = endAt;
            LaborId = laborId;
            Spot = spot;
            State = state;
            _barberService = barberServices;
        }

        public static Result<WorkOrder> Create(Guid id, Guid vehicleId, DateTimeOffset startAt, DateTimeOffset endAt, Guid laborId, Spot spot, List<BarberService> barberServices)
        {
            if (id == Guid.Empty)
            {
                return WorkOrderErrors.WorkOrderIdRequired;
            }

            if (vehicleId == Guid.Empty)
            {
                return WorkOrderErrors.VehicleIdRequired;
            }

            if (barberServices == null || barberServices.Count == 0)
            {
                return WorkOrderErrors.BarberServicesRequired;
            }

            if (laborId == Guid.Empty)
            {
                return WorkOrderErrors.LaborIdRequired;
            }

            if (endAt <= startAt)
            {
                return WorkOrderErrors.InvalidTiming;
            }

            if (!Enum.IsDefined(spot))
            {
                return WorkOrderErrors.SpotInvalid;
            }

            return new WorkOrder(id, vehicleId, startAt, endAt, laborId, spot, WorkOrderState.Scheduled, barberServices);
        }

        public Result<Updated> AddBarberService(BarberService barberService)
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.Readonly;
            }

            if (_barberService.Any(r => r.Id == barberService.Id))
            {
                return WorkOrderErrors.BarberServiceAlreadyAdded;
            }

            _barberService.Add(barberService);

            return Result.Updated;
        }

        public Result<Updated> UpdateTiming(DateTimeOffset startAt, DateTimeOffset endAt)
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.TimingReadonly(Id.ToString(), State);
            }

            if (endAt <= startAt)
            {
                return WorkOrderErrors.InvalidTiming;
            }

            StartAtUtc = startAt;
            EndAtUtc = endAt;

            return Result.Updated;
        }

        public Result<Updated> UpdateLabor(Guid laborId)
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.Readonly;
            }

            if (laborId == Guid.Empty)
            {
                return WorkOrderErrors.LaborIdEmpty(Id.ToString());
            }

            LaborId = laborId;

            return Result.Updated;
        }

        public Result<Updated> UpdateState(WorkOrderState newState)
        {
            if (!CanTransitionTo(newState))
            {
                return WorkOrderErrors.InvalidStateTransition(State, newState);
            }

            State = newState;

            return Result.Updated;
        }

        public bool IsEditable => State is not (WorkOrderState.Completed or WorkOrderState.Cancelled or WorkOrderState.InProgress);

        public bool CanTransitionTo(WorkOrderState newStatus)
        {
            return (State, newStatus) switch
            {
                (WorkOrderState.Scheduled, WorkOrderState.InProgress) => true,
                (WorkOrderState.InProgress, WorkOrderState.Completed) => true,
                (_, WorkOrderState.Cancelled) when State != WorkOrderState.Completed => true,
                _ => false
            };
        }

        public Result<Updated> Cancel()
        {
            if (!CanTransitionTo(WorkOrderState.Cancelled))
            {
                return WorkOrderErrors.InvalidStateTransition(State, WorkOrderState.Cancelled);
            }

            State = WorkOrderState.Cancelled;
            return Result.Updated;
        }

        public Result<Updated> ClearBarberServices()
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.Readonly;
            }

            _barberService.Clear();

            return Result.Updated;
        }

        public Result<Updated> UpdateSpot(Spot newSpot)
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.Readonly;
            }

            if (!Enum.IsDefined(newSpot))
            {
                return WorkOrderErrors.SpotInvalid;
            }

            Spot = newSpot;

            return Result.Updated;
        }
    }

}
