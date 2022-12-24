using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Mihaylov.Common.Abstract.Infrastructure.Interfaces;

namespace Mihaylov.Common.Abstract.Infrastructure.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            user = httpContextAccessor.HttpContext?.User;
        }

        public string GetUserName()
        {
            return user?.Identity?.Name;
        }

        public string GetId()
        {
            return user?.Claims
                             .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                             ?.Value;
        }
    }
}
