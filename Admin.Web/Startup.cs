using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Context;
using Auth.Core.ViewModels;
using Imanage.Shared.AspNetCore.AuditNet;
using Imanage.Shared.AspNetCore.Filters;
using Imanage.Shared.DI;
using Imanage.Shared.Extensions;
using Imanage.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Auth.Web
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSwagger("Authentication Service");
            AddCustomDbContext(services);
            AddCustomIdentity(services);
            ConfigureOpenIddict(services);
            ConfigureDIService(services);

            var connectionString = Configuration.GetConnectionString("Default");
            services.ConfigureAudit(connectionString)
                .AddMvc(_ =>
                {
                    _.AddAudit();
                    _.Filters.Add(typeof(ImanageExceptionAttribute));
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x =>
            {
                x.WithOrigins(Configuration["AllowedCorsOrigin"]
                  .Split(",", StringSplitOptions.RemoveEmptyEntries)
                  .Select(o => o.RemovePostFix("/"))
                  .ToArray())
             .AllowAnyMethod()
             .AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseCustomSwaggerApi();
            app.UseMvc();

            //InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        public IServiceCollection AddCustomDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");

            services.AddDbContextPool<ImanageAuthDbContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b => b.MigrationsAssembly(typeof(ImanageAuthDbContext).FullName));
            });

            return services;
        }

        public IServiceCollection AddCustomIdentity(IServiceCollection services)
        {

            var authsettings = new AppSettings();
            Configuration.Bind("Appsettings", authsettings);

            services.AddIdentity<ImanageUser, ImanageRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = authsettings.MinimumPasswordLength;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(authsettings.LockoutMinutes);
                options.Lockout.MaxFailedAccessAttempts = authsettings.MaxLockoutAttempt;
            })
            .AddTokenProvider<PhoneNumberTokenProvider<ImanageUser>>("Phone2FA")
            .AddEntityFrameworkStores<ImanageAuthDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24);
            });

            return services;
        }
    }
}
