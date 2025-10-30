using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Scheduling.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Scheduling.Queries.GetDailyScheduleQuery
{
    public sealed record GetDailyScheduleQuery(
    TimeZoneInfo TimeZone,
    DateOnly ScheduleDate,
    Guid? LaborId = null) : ICachedQuery<Result<ScheduleDto>>
    {
        public string CacheKey => $"work-order:{ScheduleDate:yyyy-MM-dd}:labor={LaborId?.ToString() ?? "-"}";

        public string[] Tags => ["work-order"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
