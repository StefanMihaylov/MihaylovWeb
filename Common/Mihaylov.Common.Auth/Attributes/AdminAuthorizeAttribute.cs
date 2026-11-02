using Microsoft.AspNetCore.Authorization;

namespace Mihaylov.Common
{
    /// <summary>
    /// Specifies that access to a controller or action is restricted to users with the Admin access level.
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the AdminAuthorizeAttribute class with the admin authentication scheme and
        /// policy.
        /// </summary>
        public AdminAuthorizeAttribute()
        {
            AuthenticationSchemes = UserConstants.AuthenticationScheme;
            Policy = AccessLevel.Admin.ToString();
        }
    }
}
