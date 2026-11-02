using Microsoft.AspNetCore.Authorization;

namespace Mihaylov.Common
{
    /// <summary>
    /// Specifies an authorization attribute that restricts access to users with the default authentication scheme and
    /// user-level policy.
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the UserAuthorizeAttribute class with the default authentication scheme and
        /// user access policy.
        /// </summary>
        public UserAuthorizeAttribute()
        {
            AuthenticationSchemes = UserConstants.AuthenticationScheme;
            Policy = AccessLevel.User.ToString();
        }
    }
}
