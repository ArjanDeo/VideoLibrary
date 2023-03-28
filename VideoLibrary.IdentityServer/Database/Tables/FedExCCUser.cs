using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoLibrary.IdentityServer.Database.Tables
{
    public class FedExCCUser : IdentityUser<Guid>
    {
        public FedExCCUser()
        {
        }

        public FedExCCUser(string userName) : base(userName)
        {
        }

        #region Identity Server Required Fields
        [Column("userId")]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        [Column("userEmail")]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Column("userPassword")]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        [Column("userPhone")]
        public string userPhone { get; set; }

        [Column("userFullName")]
        public string userFullName { get; set; }

        [Column("userIsActive")]
        public bool? userIsActive { get; set; }

        [Column("createdDT")]
        public DateTime CreatedDT { get; set; }

        [Column("vendorid")]
        public int? vendorid { get; set; }

        [Column("companyId")]
        public int companyId { get; set; }

        [Column("normalizedUserEmail")]
        public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }

        [Column("concurrencyStamp")]
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

        [Column("securityStamp")]
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }

        [Column("userName")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [Column("normalizedUserName")]
        public override string NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }

        [Column("emailConfirmed")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [NotMapped]
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }

        [NotMapped]
        public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }

        [NotMapped]
        public override DateTimeOffset? LockoutEnd { get => base.LockoutEnd; set => base.LockoutEnd = value; }

        [NotMapped]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [NotMapped]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }

        [NotMapped]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }

        #endregion
    }
}
