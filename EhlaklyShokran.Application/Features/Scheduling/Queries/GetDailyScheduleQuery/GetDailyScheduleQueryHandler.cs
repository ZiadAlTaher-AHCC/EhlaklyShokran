using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Labors.Mappers;
using EhlaklyShokran.Application.Features.Scheduling.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using EhlaklyShokran.Domain.Workorders.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EhlaklyShokran.Application.Features.BarberTasks.Mappers;

namespace EhlaklyShokran.Application.Features.Scheduling.Queries.GetDailyScheduleQuery
{
    public class GetDailyScheduleQueryHandler(
    IApplicationDbContext context,
    TimeProvider datetime)
    : IRequestHandler<GetDailyScheduleQuery, Result<ScheduleDto>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly TimeProvider _datetime = datetime;
        public async Task<Result<ScheduleDto>> Handle(GetDailyScheduleQuery query, CancellationToken cancellationToken)
        {
            var localStart = query.ScheduleDate.ToDateTime(TimeOnly.MinValue);
            var localEnd = localStart.AddDays(1);


            var utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, query.TimeZone);
            var utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd, query.TimeZone);

            var workOrders = await _context.WorkOrders
                .Where(w =>
                    w.StartAtUtc < utcEnd &&
                    w.EndAtUtc > utcStart &&
                    (query.LaborId == null || w.LaborId == query.LaborId))
                .Include(w => w.BarberTasks)
                .Include(w => w.Labor)
                .ToListAsync(cancellationToken);

            var now = TimeZoneInfo.ConvertTime(_datetime.GetUtcNow(), query.TimeZone);

            var result = new ScheduleDto
            {
                OnDate = query.ScheduleDate,
                EndOfDay = localEnd < now,
                Spots = []
            };

            foreach (var spot in Enum.GetValues<Spot>())
            {
                var current = localStart;
                var slots = new List<AvailabilitySlotDto>();

                var woBySpot = workOrders
                    .Where(w => w.Spot == spot)

                    .OrderBy(w => w.StartAtUtc)
                    .ToList();

                while (current < localEnd)
                {
                    var next = current.AddMinutes(15);
                    var startUtc = TimeZoneInfo.ConvertTimeToUtc(current, query.TimeZone);
                    var endUtc = TimeZoneInfo.ConvertTimeToUtc(next, query.TimeZone);

                    var wo = woBySpot.FirstOrDefault(w =>
                        w.StartAtUtc < endUtc && w.EndAtUtc > startUtc);

                    if (wo != null)
                    {
                        if (!slots.Any(s => s.WorkOrderId == wo.Id))
                        {
                            slots.Add(new AvailabilitySlotDto
                            {
                                WorkOrderId = wo.Id,
                                Spot = spot,
                                StartAt = wo.StartAtUtc,
                                EndAt = wo.EndAtUtc,
                                Labor = wo.Labor!.ToDto(),
                                IsOccupied = true,
                                BarberTasks = [.. wo.BarberTasks.ToList().ConvertAll(rt => rt.ToDto())],
                                WorkOrderLocked = !wo.IsEditable,
                                State = wo.State,
                                IsAvailable = false
                            });
                        }
                    }
                    else
                    {
                        slots.Add(new AvailabilitySlotDto
                        {
                            Spot = spot,
                            StartAt = startUtc,
                            EndAt = endUtc,
                            WorkOrderLocked = false,
                            IsAvailable = current >= now
                        });
                    }

                    current = next;
                }

                result.Spots.Add(new SpotDto
                {
                    Spot = spot,
                    Slots = slots
                });
            }

            return result;
        }

    }
}
