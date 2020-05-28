
using Imanage.Shared.Helpers;
using Imanage.Shared.Statics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imanage.Shared.Models.Map
{
    public class MyAppUserClaimMap : IEntityTypeConfiguration<ImanageUserClaim>
    {
       private static int counter = 0;

        public void Configure(EntityTypeBuilder<ImanageUserClaim> builder)
        {
            builder.ToTable("ImanageUserClaim");
            builder.HasKey(c => c.Id);

            builder.HasData(ADMINPermissionData());
            builder.HasData(SYAdminPermissionData());
            builder.HasData(SuperAdminPermissionData());
            builder.HasData(EMPermissionData());
           


        }

       
        private static IEnumerable<ImanageUserClaim> SYAdminPermissionData()
        {
            var sysAdminPermissions = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelpers.SYS_ADMIN)
                .SelectMany(x => x.Value);


            foreach (var item in sysAdminPermissions)
            {
                yield return new ImanageUserClaim
                {
                    Id = ++counter,
                    ClaimType = nameof(Permission),
                    ClaimValue = item.ToString(),
                    UserId = Defaults.AdminId
                };
            }
        }

        private static IEnumerable<ImanageUserClaim> SuperAdminPermissionData()
        {
            var superAdminPermission = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelpers.SUPER_ADMIN)
                .SelectMany(x => x.Value);


            foreach (var item in superAdminPermission)
            {
                yield return new ImanageUserClaim
                {
                    Id = ++counter,
                    ClaimType = nameof(Permission),
                    ClaimValue = item.ToString(),
                    UserId = Defaults.SuperAdminId
                };
            }
        }

        

        private static IEnumerable<ImanageUserClaim> ADMINPermissionData()
        {
            var adminPermission = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelpers.Admin_User)
                .SelectMany(x => x.Value);


            foreach (var item in adminPermission)
            {
                yield return new ImanageUserClaim
                {
                    Id = ++counter,
                    ClaimType = nameof(Permission),
                    ClaimValue = item.ToString(),
                    UserId = Defaults.EstateManagerUserId
                };
            }
        }

        private static IEnumerable<ImanageUserClaim> EMPermissionData()
        {
            var eMPermission = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelpers.Estate_Manager)
                .SelectMany(x => x.Value);


            foreach (var item in eMPermission)
            {
                yield return new ImanageUserClaim
                {
                    Id = ++counter,
                    ClaimType = nameof(Permission),
                    ClaimValue = item.ToString(),
                    UserId = Defaults.EstateManagerUserId
                };
            }
        }
    }
}
