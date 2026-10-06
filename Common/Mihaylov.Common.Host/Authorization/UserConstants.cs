using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Mihaylov.Common;

/// <summary>
/// Provides constants and ClaimsPrincipal extension methods for user identity and role checks.
/// </summary>
public static class UserConstants
{
    /// <summary>
    /// Name of the administrator role used for authorization.
    /// </summary>
    public const string AdminRole = "Administrator";

    /// <summary>
    /// Default authentication scheme name for JWT bearer authentication.
    /// </summary>
    public const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

    /// <summary>
    /// Gets the user's identifier from the NameIdentifier claim as a Guid.
    /// </summary>
    /// <param name="user">The claims principal to retrieve the identifier from.</param>
    /// <returns>The identifier as a Guid, or Guid.Empty if the NameIdentifier claim is missing or empty.</returns>
    public static Guid GetId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(id))
        {
            return new Guid(id);
        }
        
        return Guid.Empty;
    }

    /// <summary>
    /// Determines whether the specified ClaimsPrincipal is in the administrator role.
    /// </summary>
    /// <param name="user">The ClaimsPrincipal to check for membership in the administrator role.</param>
    /// <returns>true if the principal is in the administrator role; otherwise, false.</returns>
    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole(AdminRole);
    }

    /// <summary>
    /// Determines whether the principal represents the specified user identifier or has administrator privileges.
    /// </summary>
    /// <param name="user">The claims principal to evaluate.</param>
    /// <param name="userId">The user identifier to compare against the principal's identifier.</param>
    /// <returns>True if the principal is an administrator or its identifier equals the specified userId; otherwise, false.</returns>
    public static bool IsOwnId(this ClaimsPrincipal user, Guid userId)
    {
        if (user.IsAdmin())
        {
            return true;
        }

        return user.GetId() == userId;
    }
}
