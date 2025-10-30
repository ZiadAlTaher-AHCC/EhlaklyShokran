using EhlaklyShokran.Application.Features.Identity.Dtos;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string? policyName);

        Task<Result<AppUserDto>> AuthenticateAsync(string email, string password);

        Task<Result<AppUserDto>> GetUserByIdAsync(string userId);

        Task<string?> GetUserNameAsync(string userId);
    }
}
