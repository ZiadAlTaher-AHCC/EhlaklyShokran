using EhlaklyShokran.Domain.BarberTasks;
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
using EhlaklyShokran.Domain.Customers;

namespace EhlaklyShokran.Domain.WorkOrders
{

    public sealed class WorkOrder : AuditableEntity
    {
        public DateTimeOffset StartAtUtc { get; private set; }
        public DateTimeOffset EndAtUtc { get; private set; }
        public Guid LaborId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Spot Spot { get; private set; }
        public WorkOrderState State { get; private set; }
        public Employee? Labor { get; set; }
        public Customer? Customer { get; set; }
        public Invoice? Invoice { get; set; }
        public decimal? Discount { get; private set; }
        public decimal? Tax { get; private set; }
        public decimal? TotalPartsCost => _BarberTask.SelectMany(rt => rt.Cosmetics).Sum(p => p.Cost);
        public decimal? TotalLaborCost => _BarberTask.Sum(rt => rt.LaborCost);
        public decimal? Total => (TotalPartsCost ?? 0) + (TotalLaborCost ?? 0);

        private readonly List<BarberTask> _BarberTask = [];
        public IEnumerable<BarberTask> BarberTasks => _BarberTask.AsReadOnly();

        private WorkOrder()
        { }

        private WorkOrder(Guid id, Guid customerId, DateTimeOffset startAt, DateTimeOffset endAt, Guid laborId, Spot spot, WorkOrderState state, List<BarberTask> BarberTasks)
            : base(id)
        {
            StartAtUtc = startAt;
            EndAtUtc = endAt;
            LaborId = laborId;
            Spot = spot;
            State = state;
            CustomerId = customerId;
            _BarberTask = BarberTasks;
        }

        public static Result<WorkOrder> Create(Guid id, Guid customerId, DateTimeOffset startAt, DateTimeOffset endAt, Guid laborId, Spot spot, List<BarberTask> BarberTasks)
        {
            if (id == Guid.Empty)
            {
                return WorkOrderErrors.WorkOrderIdRequired;
            }

            if (customerId == Guid.Empty)
            {
                return WorkOrderErrors.CustomerIdRequired;
            }

            if (BarberTasks == null || BarberTasks.Count == 0)
            {
                return WorkOrderErrors.BarberTasksRequired;
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

            return new WorkOrder(id, customerId, startAt, endAt, laborId, spot, WorkOrderState.Scheduled, BarberTasks);
        }

        public Result<Updated> AddBarberTask(BarberTask BarberTask)
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.Readonly;
            }

            if (_BarberTask.Any(r => r.Id == BarberTask.Id))
            {
                return WorkOrderErrors.BarberTaskAlreadyAdded;
            }

            _BarberTask.Add(BarberTask);

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

        public Result<Updated> ClearBarberTasks()
        {
            if (!IsEditable)
            {
                return WorkOrderErrors.Readonly;
            }

            _BarberTask.Clear();

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
