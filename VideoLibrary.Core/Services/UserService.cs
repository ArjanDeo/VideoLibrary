using VideoLibrary.Core.Helpers;
using VideoLibrary.Interfaces;
using VideoLibrary.Models.Constants;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace VideoLibrary
{
    public class UserService : IUserService
    {
        public Task<Guid> GetUserIdAsync(ClaimsPrincipal User)
        {
            Guid.TryParse(User.FindFirstValue(ClaimConstants.USER_ID_CLAIM), out Guid UserId);

            return Task.FromResult(UserId);
        }

        public Task<T> GetClaimAsync<T>(ClaimsPrincipal User, string ClaimType)
        {
            T Result = GenericCastHelper.To<T>(User.FindFirstValue(ClaimType));

            return Task.FromResult(Result);
        }
    }
}
