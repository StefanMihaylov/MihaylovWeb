using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
    /// <param name="values">Optional extra route values (route parameters go into the path, the rest into the query string); 
    /// <c>area</c> is always taken from MenuRoute property of <paramref name="menu"/>.</param>
    /// <returns>The URL, or <c>null</c> when <paramref name="menu"/> or its route is <c>null</c>
    /// or the route has neither a controller nor a page.</returns>
    public static string Href(this IUrlHelper urlHelper, MenuItem menu, object values = null)
    {
        var url = urlHelper.Href(menu?.MenuRoute, values);

        return url;
    }

    /// <summary>
    /// Builds a URL from a menu route. A route with a controller becomes an action URL, otherwise a route with a
    /// page becomes a Razor Pages URL. The area is passed in both cases.
    /// </summary>
    /// <param name="urlHelper">The URL helper used to generate the URL.</param>
    /// <param name="route">The route to convert, may be <c>null</c>.</param>
    /// <param name="values">Optional extra route values (route parameters go into the path, the rest into the query string); 
    /// <c>area</c> is always taken from <paramref name="route"/>.</param>
    /// <returns>The URL, or <c>null</c> when <paramref name="route"/> is <c>null</c> or has neither a controller
    /// nor a page.</returns>
    public static string Href(this IUrlHelper urlHelper, IMenuRoute route, object values = null)
    {
        if (route == null)
        {
            return null;
        }

        var routeValues = new RouteValueDictionary(values) { ["area"] = route.Area };

        if (route.Controller == null)
        {
            if (route.Page == null)
            {
                return null;
            }


            var pageUrl = urlHelper.Page(route.Page, routeValues);
            return pageUrl;
        }

        var actionUrl = urlHelper.Action(route.Action, route.Controller, routeValues);
        return actionUrl;
    }

    /// <summary>Returns the URL of a static file of the RCL that contains <typeparamref name="TModule"/>, 
    /// for example <c>js/person.js</c>.
    /// </summary>
    public static string ModuleFile<TModule>(this IUrlHelper urlHelper, string path)
    {
        return urlHelper.Content($"~/_content/{typeof(TModule).Assembly.GetName().Name}/{path}");
    }
}
