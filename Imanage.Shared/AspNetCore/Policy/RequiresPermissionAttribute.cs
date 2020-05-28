using Imanage.Shared.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.AspNetCore.Policy
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        public RequiresPermissionAttribute(params Permission[] permissions)
          : base(typeof(RequiresPermissionAttributeImpl))
        {
            Arguments = new[] { new PermissionsAuthorizationRequirement(permissions) };
        }

        private class RequiresPermissionAttributeImpl : Attribute, IAsyncResourceFilter
        {
            private readonly ILogger _logger;
            private readonly IAuthorizationService _authService;
            private readonly PermissionsAuthorizationRequirement _requiredPermissions;

            public RequiresPermissionAttributeImpl(ILogger<RequiresPermissionAttribute> logger,
                                            IAuthorizationService authService,
                                            PermissionsAuthorizationRequirement requiredPermissions)
            {
                _logger = logger;
                _authService = authService;
                _requiredPermissions = requiredPermissions;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context,
                                                       ResourceExecutionDelegate next)
            {
                if (!(await _authService.AuthorizeAsync(context.HttpContext.User,
                                            context.ActionDescriptor.ToString(),
                                            _requiredPermissions)).Succeeded)
                {
                    context.Result = new ForbidResult(JwtBearerDefaults.AuthenticationScheme);
                    return;
                }

                await next();
            }
        }
    }
}
