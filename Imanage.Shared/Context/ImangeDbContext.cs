using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Imanage.Shared.Context;
using Imanage.Shared.Models;

namespace Dafmis.Shared.Context
{
    public class ImangeDbContext:AuthDbContext
    {
        public ImangeDbContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore(typeof(ImanageUser));
            modelBuilder.Ignore(typeof(ImanageRole));
            modelBuilder.Ignore(typeof(ImanageUserClaim));
            modelBuilder.Ignore(typeof(ImanageUserRole));
            modelBuilder.Ignore(typeof(ImanageUserLogin));
            modelBuilder.Ignore(typeof(ImanageRoleClaim));
            modelBuilder.Ignore(typeof(ImanageUserToken));
        }
    }
}
