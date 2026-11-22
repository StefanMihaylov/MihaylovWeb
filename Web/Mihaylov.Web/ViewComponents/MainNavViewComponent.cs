using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Common;

namespace Mihaylov.Web.ViewComponents;

public class MainNavViewComponent(IModuleCatalog catalog, IAuthorizationService authService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var visible = new List<MenuItem>();

        var menuItems = catalog.MenuModules.SelectMany(m => m.Menus).OrderBy(i => i.Order).ToList();
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
            Login = catalog.Login,
            MenuItems = visible,
        };

        return View(model);
    }

    private async Task<MenuItem> FilterAsync(MenuItem menuItem)
    {
        if (menuItem.AccessLevel != AccessLevel.Public)
        {
            var authorization = await authService.AuthorizeAsync(UserClaimsPrincipal, menuItem.AccessLevel.ToString()).ConfigureAwait(false);
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

        return new MenuItem(menuItem.Order, menuItem.Title, menuItem.MenuRoute, menuItem.AccessLevel, menuItem.Icon,
                                kids, menuItem.DividerBefore, menuItem.IsHomeMenu);
    }
}