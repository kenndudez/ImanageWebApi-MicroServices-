
using Imanage.Shared.Enums;
using Imanage.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Imanage.Shared.Models.Map
{
    public class MyAppRoleMap : IEntityTypeConfiguration<ImanageRole>
    {
        public void Configure(EntityTypeBuilder<ImanageRole> builder)
        {
            builder.ToTable("ImanageRole");
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<ImanageRole> builder)
        {
            var roles = new ImanageRole[]
            {
                new ImanageRole
                {
                    Id = RoleHelpers.SYS_ADMIN_ID(),
                    Name = RoleHelpers.SYS_ADMIN.ToString(),
                    NormalizedName = UserRoles.SYS_ADMIN.ToString()
                },
                new ImanageRole
                {
                    Id = RoleHelpers.SUPER_ADMIN_ID(),
                    Name = RoleHelpers.SUPER_ADMIN.ToString(),
                    NormalizedName = UserRoles.SUPER_ADMIN.ToString()
                },
                new  ImanageRole
                {
                    Id = RoleHelpers.LandLord_ID(),
                    Name = RoleHelpers.LandLord.ToString(),
                    NormalizedName = UserRoles.LandLord.ToString()
                },
               
                new  ImanageRole
                {
                    Id = RoleHelpers.Estate_Manager_ID(),
                    Name=  RoleHelpers.Estate_Manager.ToString(),
                    NormalizedName = UserRoles.Estate_Manager.ToString()
                },          
            };

            builder.HasData(roles);
        }
    }
}