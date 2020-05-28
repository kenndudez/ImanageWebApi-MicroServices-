using Dafmis.Shared.Identity;
using Microsoft.AspNetCore.Http;

namespace Imanage.Shared.AspNetCore
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public UserPrincipal GetCurrentUser
        {
            get
            {
                if (_httpContext.HttpContext != null && _httpContext.HttpContext.User != null)
                {
                    return new UserPrincipal(_httpContext.HttpContext.User);
                }

                return null;
            }
        }
    }

    public interface ICurrentUserService
    {
        UserPrincipal GetCurrentUser { get; }
    }
}
