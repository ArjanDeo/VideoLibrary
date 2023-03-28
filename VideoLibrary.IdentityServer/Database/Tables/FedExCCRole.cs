using System;
using Microsoft.AspNetCore.Identity;

namespace VideoLibrary.IdentityServer.Database.Tables
{
    public class FedExCCRole : IdentityRole<Guid>
    {
        public FedExCCRole()
        {
        }

        public FedExCCRole(string roleName) : base(roleName)
        {
        }
    }
}
