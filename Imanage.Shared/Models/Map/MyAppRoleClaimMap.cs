using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imanage.Shared.Models.Map
{
    public class MyAppRoleClaimMap : IEntityTypeConfiguration<ImanageRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ImanageRoleClaim> builder)
        {
            builder.ToTable("ImanageRoleClaim");
        }
    }
}
