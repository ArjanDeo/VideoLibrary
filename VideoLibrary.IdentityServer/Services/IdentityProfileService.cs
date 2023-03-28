using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace VideoLibrary.IdentityServer.Services
{
    public class IdentityProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await Task.FromResult(Task.CompletedTask);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.FromResult(Task.CompletedTask);
        }
    }
}
