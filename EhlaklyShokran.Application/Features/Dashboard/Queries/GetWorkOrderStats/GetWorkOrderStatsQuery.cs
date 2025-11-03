using EhlaklyShokran.Application.Features.Dashboard.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Dashboard.Queries.GetWorkOrderStats
{
    public sealed record GetWorkOrderStatsQuery(DateOnly Date) : IRequest<Result<TodayWorkOrderStatsDto>>;
}
