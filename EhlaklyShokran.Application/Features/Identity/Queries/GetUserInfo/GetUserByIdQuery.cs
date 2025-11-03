using EhlaklyShokran.Application.Features.Identity.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Identity.Queries.GetUserInfo
{
    public sealed record GetUserByIdQuery(string? UserId) : IRequest<Result<AppUserDto>>;
}
