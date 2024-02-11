using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Mihaylov.Common.Host.Authorization
{
    public static class UserConstants
    {
        public const string AdminRole = "Administrator";

        public const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;


        public static Guid GetId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                return new Guid(id);
            }
            
            return Guid.Empty;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminRole);
        }

        public static bool IsOwnId(this ClaimsPrincipal user, Guid userId)
        {
            if (user.IsAdmin())
            {
                return true;
            }

            return user.GetId() == userId;
        }
    }
}
