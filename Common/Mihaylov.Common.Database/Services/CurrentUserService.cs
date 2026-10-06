using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Mihaylov.Common;

/// <summary>
/// Exposes the current authenticated user's name and identifier as obtained from the HTTP context.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal user;

    /// <summary>
    /// Initializes a new CurrentUserService that captures the current HttpContext.User from the provided
    /// IHttpContextAccessor.
    /// </summary>
    /// <param name="httpContextAccessor">IHttpContextAccessor used to obtain the current HttpContext and its User (ClaimsPrincipal).</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        user = httpContextAccessor.HttpContext?.User;
    }

    /// <summary>
    /// Retrieves the current principal's identity name, or null if unavailable.
    /// </summary>
    /// <returns>The current user's identity name, or null if the user or identity is null.</returns>
    public string GetUserName()
    {
        return user?.Identity?.Name;
    }

    /// <summary>
    /// Gets the current user's name identifier claim value (ClaimTypes.NameIdentifier).
    /// </summary>
    /// <returns>The name identifier claim value, or null if the user or claim is not present.</returns>
    public string GetId()
    {
        return user?.Claims
                         .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                         ?.Value;
    }
}
