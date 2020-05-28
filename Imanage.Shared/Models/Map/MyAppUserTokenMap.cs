using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Models.Map
{
    public class MyAppUserTokenMap: IEntityTypeConfiguration<ImanageUserToken>
    {
        public void Configure(EntityTypeBuilder<ImanageUserToken> builder)
        {
            builder.ToTable("ImanageUserToken");
            builder.HasKey(p => p.UserId);
        }
    }
}
