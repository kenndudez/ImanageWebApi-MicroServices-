using System;
using System.Collections.Generic;
using Imanage.Shared.Helpers;
using Imanage.Shared.Statics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imanage.Shared.Models.Map
{
    public class MyAppUserRoleMap : IEntityTypeConfiguration<ImanageUserRole>
    {
        public void Configure(EntityTypeBuilder<ImanageUserRole> builder)
        {
            builder.ToTable("ImanageUserRole");
            builder.HasKey(p => new { p.UserId, p.RoleId });
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<ImanageUserRole> builder)
        {
            List<ImanageUserRole> dataList = new List<ImanageUserRole>()
            {
                new ImanageUserRole()
                {
                    UserId = Defaults.AdminId,
                    RoleId = RoleHelpers.SYS_ADMIN_ID(),
                },

                new ImanageUserRole()
                {
                    UserId = Defaults.SuperAdminId,
                    RoleId = RoleHelpers.SUPER_ADMIN_ID(),
                },

                new ImanageUserRole()
                {
                    UserId = Defaults.BasicUserId,
                    RoleId = RoleHelpers.BASIC_ID(),
                },

                new ImanageUserRole()
                {
                    UserId = Defaults.EstateManagerUserId,
                    RoleId = RoleHelpers.Estate_Manager_ID(),
                },

                new ImanageUserRole()
                {
                    UserId = Defaults.LandLordUserId,
                    RoleId = RoleHelpers.LandLord_ID(),
                },

               new ImanageUserRole()
                {
                    UserId = Defaults.GeneralUserId,
                    RoleId = RoleHelpers.GENERAL_ID(),
                },
            };

            builder.HasData(dataList);
        }
    }
}