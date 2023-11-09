using Microsoft.AspNetCore.Authorization;

namespace Mihaylov.Users.Data
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute()
        {
            this.AuthenticationSchemes = UserConstants.AuthenticationScheme;
        }
    }
}
