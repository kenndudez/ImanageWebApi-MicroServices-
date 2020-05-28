using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Models.Map
{
    public class MyAppUserLoginMap : IEntityTypeConfiguration<ImanageUserLogin>
    {
        public void Configure(EntityTypeBuilder<ImanageUserLogin> builder)
        {
            builder.ToTable("ImanageUserLogin");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasKey(u => new { u.LoginProvider, u.ProviderKey });

        }
    }
}
