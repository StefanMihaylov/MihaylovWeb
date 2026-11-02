using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common;

/// <summary>
/// Reads the current HTTP user from the service provider.
/// </summary>
public static class ProviderExtensions
{
    /// <summary>
    /// Gets the user of the current HTTP request.
    /// </summary>
    /// <param name="sp">The service provider of the current scope.</param>
    /// <returns>The user, or <c>null</c> outside a request or when <c>IHttpContextAccessor</c> is not registered.</returns>
    public static ClaimsPrincipal GetUser(this IServiceProvider sp)
    {
        ClaimsPrincipal user = sp.GetService<IHttpContextAccessor>()?.HttpContext?.User;

        return user;
    }
}
