using VideoLibrary.IdentityServer.Database.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VideoLibrary.IdentityServer.Database
{
    public class IdentityDatabase : IdentityDbContext<FedExCCUser, FedExCCRole, Guid>
    {
        public IdentityDatabase(DbContextOptions<IdentityDatabase> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FedExCCUser>()
                .ToTable("tbl_User", "dbo");

            builder.Entity<FedExCCRole>()
                .ToTable("tbl_Roles", "dbo");

            builder.Entity<IdentityUserRole<Guid>>()
                .ToTable("tbl_UserRoles", "dbo");

            builder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("tbl_RoleClaims", "dbo");

            builder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("tbl_UserClaims", "dbo");

        }
    }
}
