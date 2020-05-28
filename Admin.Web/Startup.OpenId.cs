using AspNet.Security.OpenIdConnect.Primitives;
using Auth.Core.Context;
using Auth.Core.Web.Helpers;
using Imanage.Shared.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auth.Web
{
    public partial class Startup
    {
        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {

            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                await OpenIddictManager.CreateClientApps(manager, cancellationToken);
            }
        }

        public void ConfigureOpenIddict(IServiceCollection services)
        {
            var authSettings = new AuthSettings();
            Configuration.Bind(nameof(AuthSettings), authSettings);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.SecretKey));

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            var x509Certificate = new X509Certificate2(Path.Combine(
                          HostingEnvironment.ContentRootPath, "auth.pfx")
                      , "idsrv3test", X509KeyStorageFlags.MachineKeySet);

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<ImanageAuthDbContext>();
                })
                .AddServer(options =>
                {
                    options.RegisterScopes(OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.Address,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIddictConstants.Scopes.Roles,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIdConnectConstants.Scopes.OpenId
                    );

                    if (!authSettings.RequireHttps)
                        options.DisableHttpsRequirement();

                    options.EnableTokenEndpoint("/api/connect/token")
                        .AllowRefreshTokenFlow()
                        .AcceptAnonymousClients()
                        .AllowPasswordFlow()
                        .SetAccessTokenLifetime(TimeSpan.FromMinutes(60))
                        .SetIdentityTokenLifetime(TimeSpan.FromMinutes(60))
                        .SetRefreshTokenLifetime(TimeSpan.FromMinutes(120))
                        .AddSigningCertificate(x509Certificate)
                        .UseJsonWebTokens();

                    //Todo: Generate auth.pfx cetificate

                    if (HostingEnvironment.IsDevelopment())
                    {
                        options.SetAccessTokenLifetime(TimeSpan.FromDays(7));
                    }
                    //if (HostingEnvironment.IsProduction()) {

                    //options.SetAccessTokenLifetime(TimeSpan.FromMinutes(60))
                    //.SetIdentityTokenLifetime(TimeSpan.FromMinutes(60))
                    //.SetRefreshTokenLifetime(TimeSpan.FromMinutes(120))
                    //.AddSigningCertificate(
                    //    assembly: typeof(Startup).GetTypeInfo().Assembly,
                    //    resource: "idsrv3test.pfx",
                    //    password: "idsrv3test")
                    //.UseJsonWebTokens();
                    //}

                    //if (HostingEnvironment.IsDevelopment())
                    //{
                    //   options.DisableHttpsRequirement();

                    //options.SetAccessTokenLifetime(TimeSpan.FromMinutes(60))
                    //    .SetIdentityTokenLifetime(TimeSpan.FromMinutes(60))
                    //    .SetRefreshTokenLifetime(TimeSpan.FromMinutes(120))

                    //services.AddOpenIddict().AddServer()
                    //.AddDevelopmentSigningCertificate();
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = options.Authority = authSettings.Authority;
                options.RequireHttpsMetadata = authSettings.RequireHttps;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Name,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role,
                    IssuerSigningKey = signingKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            //services.AddDataProtection()
            //.ProtectKeysWithCertificate(x509Certificate);
            //.DisableHttpsRequirement()
            //.UseJsonWebTokens();
            //services.AddOpenIddict().AddServer().AddDevelopmentSigningCertificate();
        }
    }
}
