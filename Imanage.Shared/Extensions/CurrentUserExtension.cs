
using Imanage.Shared.AspNetCore;
using Imanage.Shared.AspNetCore.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Imanage.Shared.Extensions
{
    public static class CurrentUserExtension
    {
        public static void AddCurrentUserPermision(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
        }

    }
}
