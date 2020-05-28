using Auth.Core.Models.Map;
using Imanage.Shared.Context;
using Imanage.Shared.Models.Map;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.Context
{
    public class ImanageAuthDbContext : AuthDbContext
    {
        public ImanageAuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
           
            
           
            modelBuilder.ApplyConfiguration(new MyAppRoleMap());
            modelBuilder.ApplyConfiguration(new MyAppUserMap());
            modelBuilder.ApplyConfiguration(new MyAppUserRoleMap());
            modelBuilder.ApplyConfiguration(new MyAppUserTokenMap());
            modelBuilder.ApplyConfiguration(new MyAppUserLoginMap());
            modelBuilder.ApplyConfiguration(new MyAppUserClaimMap());
            modelBuilder.ApplyConfiguration(new MyAppRoleClaimMap());
            modelBuilder.ApplyConfiguration(new AccountMap());

            modelBuilder.UseOpenIddict();
        }
    }
}
