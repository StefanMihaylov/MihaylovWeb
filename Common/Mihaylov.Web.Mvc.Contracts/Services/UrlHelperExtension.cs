using Microsoft.AspNetCore.Mvc;

namespace Mihaylov.Common;

public static class UrlHelperExtension
{
    public static string Href(this IUrlHelper urlHelper, MenuItem menu)
    {
        var url = urlHelper.Href(menu?.MenuRoute);

        return url;
    }

    public static string Href(this IUrlHelper urlHelper, IMenuRoute route)
    {
        if (route == null)
        {
            return null;
        }

        var url = urlHelper.Action(route.Action, route.Controller, new { route.Area });

        return url;
    }
}
