using Imanage.Shared.Helpers;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auth.Core.Web.Helpers
{
    public class OpenIddictManager
    {

        public static async Task CreateClientApps(OpenIddictApplicationManager<OpenIddictApplication> manager,
            CancellationToken cancellationToken)
        {
            if (await manager.FindByClientIdAsync(ClientAppHelper.CLIENT_WEB_ID,
                cancellationToken) == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = ClientAppHelper.CLIENT_WEB_ID,
                    ClientSecret = ClientAppHelper.CLIENT_WEB_SECRET,
                    DisplayName = "Imanage Web App",
                    PostLogoutRedirectUris = { new Uri("https://imanage.azurewebsites.net/connect/token") },
                    RedirectUris = { new Uri("https://imanage.azurewebsites.net/connect/token") },
                    Type = OpenIddictConstants.ClientTypes.Hybrid,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles
                    }
                };

                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }
    }
}
