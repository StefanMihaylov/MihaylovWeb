using System;
using System.Security.Claims;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides ClaimsPrincipal extension methods for user identity and role checks.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
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
            return user.IsAuthenticated() && user.IsInRole(UserConstants.AdminRole);
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

        /// <summary>
        /// Determines whether the specified user is authenticated.
        /// </summary>
        /// <param name="user">The claims principal to check for authentication.</param>
        /// <returns>true if the user is authenticated; otherwise, false.</returns>
        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated == true;
        }

        /// <summary>
        /// Gets the name of the current authenticated user.
        /// </summary>
        /// <param name="user">The ClaimsPrincipal representing the user.</param>
        /// <returns>The user name if available; otherwise, null.</returns>
        public static string GetName(this ClaimsPrincipal user)
        {
            return user?.Identity?.Name;
        }
    }
}
