
using Imanage.Shared.Enums;
using Imanage.Shared.Statics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Imanage.Shared.Models.Map
{
    public class MyAppUserMap : IEntityTypeConfiguration<ImanageUser>
    {
        public MyAppUserMap()
        {
        }

        public PasswordHasher<ImanageUser> Hasher { get; set; } = new PasswordHasher<ImanageUser>();

        public void Configure(EntityTypeBuilder<ImanageUser> builder)
        {
            builder.ToTable("ImanageUser");
            builder.Property(b => b.FirstName).HasMaxLength(150);
            builder.Property(b => b.LastName).HasMaxLength(150);
            builder.Property(b => b.MiddleName).HasMaxLength(150);

            SetupSuperAdmin(builder);
            SetupAdmin(builder);
        }

        private void SetupSuperAdmin(EntityTypeBuilder<ImanageUser> builder)
        {
            var sysUser = new ImanageUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                FirstName = "Imanage",
                LastName = "Estate Manager",
                Id = Defaults.SysUserId,
                LastLoginDate = DateTime.Now,
                Email = Defaults.SysUserEmail,
                EmailConfirmed = true,
                Gender = (int)Gender.MALE,
                NormalizedEmail = Defaults.SysUserEmail.ToUpper(),
                PhoneNumber = Defaults.SysUserMobile,
                UserName = Defaults.SysUserEmail,
                NormalizedUserName = Defaults.SysUserEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                UserType = UserTypes.Admin,
            };

            var superUser = new ImanageUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                Id = Defaults.SuperAdminId,
                FirstName = "Imanage",
                LastName = "Estate Manager",
                LastLoginDate = DateTime.Now,
                Email = Defaults.SuperAdminEmail,
                Gender = (int)Gender.MALE,
                EmailConfirmed = true,
                NormalizedEmail = Defaults.SuperAdminEmail.ToUpper(),
                PhoneNumber = Defaults.SuperAdminMobile,
                UserName = Defaults.SuperAdminEmail,
                NormalizedUserName = Defaults.SuperAdminEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "016020e3-5c50-40b4-9e66-bba56c9f5bf2",
                UserType = UserTypes.Admin
            };

            builder.HasData(sysUser, superUser);
        }

        private void SetupAdmin(EntityTypeBuilder<ImanageUser> builder)
        {
            var adminUser = new ImanageUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                FirstName = "System",
                LastName = "adminUser",
                Id = Defaults.AdminUserId,
                LastLoginDate = DateTime.Now,
                Email = Defaults.AdminUserEmail,
                EmailConfirmed = true,
                Gender = (int)Gender.MALE,
                NormalizedEmail = Defaults.AdminUserEmail.ToUpper(),
                PhoneNumber = Defaults.AdminUserMobile,
                UserName = Defaults.AdminUserEmail,
                NormalizedUserName = Defaults.AdminUserEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "7d728c76-1c51-491a-99db-bde6a5b0655a",
                UserType = UserTypes.Admin,
            };

            var admin = new ImanageUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                FirstName = "System",
                LastName = "admin",
                Id = Defaults.AdminId,
                LastLoginDate = DateTime.Now,
                Email = Defaults.AdminEmail,
                EmailConfirmed = true,
                Gender = (int)Gender.MALE,
                NormalizedEmail = Defaults.AdminEmail.ToUpper(),
                PhoneNumber = Defaults.AdminMobile,
                UserName = Defaults.AdminEmail,
                NormalizedUserName = Defaults.AdminEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "5725abea-38f8-45f7-a485-0805b59d078b",
                UserType = UserTypes.Admin,
            };

            builder.HasData(adminUser, admin);
        }
    }
}