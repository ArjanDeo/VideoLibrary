using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VideoLibrary.Interfaces
{
    public interface IUserService
    {
        Task<Guid> GetUserIdAsync(ClaimsPrincipal User);
        Task<T> GetClaimAsync<T>(ClaimsPrincipal User, string ClaimType);
    }
}
