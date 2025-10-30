﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Identity.Dtos
{
    public sealed record AppUserDto(string UserId, string Email, IList<string> Roles, IList<Claim> Claims);

}
