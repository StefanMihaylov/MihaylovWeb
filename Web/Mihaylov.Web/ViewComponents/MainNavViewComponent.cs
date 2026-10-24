using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Common;

namespace Mihaylov.Web.ViewComponents;

public class MainNavViewComponent(IModuleCatalog catalog, IAuthorizationService auth) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var visible = new List<MenuItem>()
        {
            catalog.Home
        };

        var menuItems = catalog.Modules.SelectMany(m => m.Menus).OrderBy(i => i.Order).ToList();
        foreach (var menuItem in menuItems)
        {
            var filtered = await FilterAsync(menuItem);
            if (filtered != null)
            {
                visible.Add(filtered);
            }
        }

        var model = new MainNavViewModel()
        {
            Home = catalog.Home,
            MenuItems = visible
        };

        return View(model);
    }

    private async Task<MenuItem> FilterAsync(MenuItem menuItem)
    {
        if (menuItem.PermissionPolicy != null)
        {
            var authorization = await auth.AuthorizeAsync(UserClaimsPrincipal, menuItem.PermissionPolicy).ConfigureAwait(false);
            if (!authorization.Succeeded)
            {
                return null;
            }
        }

        if (menuItem.Children == null)
        {
            return menuItem;
        }

        var kids = new List<MenuItem>();

        var children = menuItem.Children.OrderBy(c => c.Order).ToList();
        foreach (var child in children)
        {
            var f = await FilterAsync(child);
            if (f != null)
            {
                kids.Add(f);
            }
        }

        if (kids.Count == 0)
        {
            return null;
        }

        menuItem.Children = kids;

        return menuItem;
    }
}