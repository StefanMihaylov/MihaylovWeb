using Microsoft.AspNetCore.Authorization;

namespace Mihaylov.Common.Host.Authorization
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute()
        {
            this.AuthenticationSchemes = UserConstants.AuthenticationScheme;
        }
    }
}
