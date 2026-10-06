using Microsoft.AspNetCore.Authorization;

namespace Mihaylov.Common;

/// <summary>
/// Specifies that access to a controller or action requires JWT bearer authentication using the application's
/// configured authentication scheme.
/// </summary>
public class JwtAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of JwtAuthorizeAttribute and sets AuthenticationSchemes to
    /// UserConstants.AuthenticationScheme.
    /// </summary>
    public JwtAuthorizeAttribute()
    {
        this.AuthenticationSchemes = UserConstants.AuthenticationScheme;
    }
}
