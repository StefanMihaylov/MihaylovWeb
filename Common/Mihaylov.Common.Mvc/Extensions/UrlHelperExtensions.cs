using Microsoft.AspNetCore.Mvc;

namespace Mihaylov.Common;

/// <summary>
/// Extension methods that turn menu items and menu routes into URLs.
/// </summary>
public static class UrlHelperExtensions
{
    /// <summary>
    /// Builds the URL of a menu item from its route.
    /// </summary>
    /// <param name="urlHelper">The URL helper used to generate the URL.</param>
    /// <param name="menu">The menu item, may be <c>null</c>.</param>
    /// <returns>The URL, or <c>null</c> when <paramref name="menu"/> or its route is <c>null</c>
    /// or the route has neither a controller nor a page.</returns>
    public static string Href(this IUrlHelper urlHelper, MenuItem menu)
    {
        var url = urlHelper.Href(menu?.MenuRoute);

        return url;
    }

    /// <summary>
    /// Builds a URL from a menu route. A route with a controller becomes an action URL, otherwise a route with a
    /// page becomes a Razor Pages URL. The area is passed in both cases.
    /// </summary>
    /// <param name="urlHelper">The URL helper used to generate the URL.</param>
    /// <param name="route">The route to convert, may be <c>null</c>.</param>
    /// <returns>The URL, or <c>null</c> when <paramref name="route"/> is <c>null</c> or has neither a controller
    /// nor a page.</returns>
    public static string Href(this IUrlHelper urlHelper, IMenuRoute route)
    {
        if (route?.Controller == null)
        {
            if (route?.Page == null)
            {
                return null;
            }

            var pageUrl = urlHelper.Page(route.Page, new { route.Area });
            return pageUrl;
        }

        var actionUrl = urlHelper.Action(route.Action, route.Controller, new { route.Area });
        return actionUrl;
    }
}
