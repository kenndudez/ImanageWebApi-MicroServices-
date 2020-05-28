using Imanage.Shared.AspNetCore.Policy;
using Imanage.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Imanage.Shared.AspNetCore.Policy
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionsAuthorizationRequirement>
    {

        public PermissionAuthorizationHandler()
        {
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizationRequirement requirement)
        {
            var currentUserPermissions = context.User.Claims
                .Where(x => x.Type.Equals(nameof(Permission))).Select(x => x.Value);

            var authorized = requirement.RequiredPermissions//.AsParallel()
                .All(rp => currentUserPermissions.Contains(rp.ToString()));
            if (authorized) context.Succeed(requirement);
        }
    }
}