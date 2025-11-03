using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Application.Features.Labors.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Labors.Queries
{
    public sealed record GetLaborsQuery() : ICachedQuery<Result<List<LaborDto>>>
    {
        public string CacheKey => $"labors";
        public string[] Tags => ["labors"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
