using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mihaylov.Common;

/// <summary>
/// The target of a menu item, built from a controller action or from a Razor page model.
/// </summary>
/// <typeparam name="TController">The controller type, or the page model type for a Razor page.</typeparam>
public class MenuRoute<TController> : IMenuRoute where TController : class
{
    /// <inheritdoc />
    public string Controller { get; }

    /// <inheritdoc />
    public string Action { get; }

    /// <inheritdoc />
    public string Area { get; }

    /// <inheritdoc />
    public string Page { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuRoute{TController}"/> class for an asynchronous controller action.
    /// </summary>
    /// <param name="actionFunc">A call to the action, for example <c>c => c.Index()</c>. If it is not a method call, the action is <c>Index</c>.</param>
    public MenuRoute(Expression<Func<TController, Task>> actionFunc)
    {
        var type = typeof(TController);

        Controller = GetController(type);
        Action = GetAction(actionFunc);
        Area = GetArea(type);
        Page = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuRoute{TController}"/> class for a controller action.
    /// </summary>
    /// <param name="actionFunc">A call to the action, for example <c>c => c.Index()</c>. If it is not a method call, the action is <c>Index</c>.</param>
    public MenuRoute(Expression<Action<TController>> actionFunc)
    {
        var type = typeof(TController);

        Controller = GetController(type);
        Action = GetAction(actionFunc);
        Area = GetArea(type);
        Page = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuRoute{TController}"/> class for a Razor page.
    /// The page and area are taken from the namespace of <typeparamref name="TController"/>.
    /// </summary>
    public MenuRoute()
    {
        var type = typeof(TController); // page

        (string page, string area) = GetPage(type);

        Controller = null;
        Action = null;
        Area = area;
        Page = page;
    }


    private static string GetArea(Type type)
    {
        return type.GetCustomAttribute<AreaAttribute>()?.RouteValue ?? string.Empty;
    }

    private static string GetController(Type type)
    {
        var controller = type.Name;
        if (controller.EndsWith("Controller"))
        {
            controller = controller.Substring(0, controller.Length - "Controller".Length);
        }

        return controller;
    }

    private static string GetAction(Expression<Action<TController>> actionFunc)
    {
        var call = actionFunc.Body as MethodCallExpression;
        string action = call?.Method?.Name ?? "Index";

        return action;
    }

    private static string GetAction(Expression<Func<TController, Task>> actionFunc)
    {
        var call = actionFunc.Body as MethodCallExpression;
        string action = call?.Method?.Name ?? "Index";

        return action;
    }

    private static (string Page, string Area) GetPage(Type pageModelType)
    {
        // "Mihaylov.Web.Areas.Identity.Pages.Account" -> ["Mihaylov", "Web", "Areas", ...]
        string[] parts = (pageModelType.Namespace ?? "").Split('.');

        string area = "";
        int pagesIndex = -1;

        // Pattern: Areas.<AreaName>.Pages
        for (int i = 0; i + 2 < parts.Length; i++)
        {
            if (parts[i] == "Areas" && parts[i + 2] == "Pages")
            {
                area = parts[i + 1];
                pagesIndex = i + 2;
                break;
            }
        }

        // No area: the first "Pages" segment
        if (pagesIndex < 0)
        {
            pagesIndex = Array.IndexOf(parts, "Pages");
        }

        if (pagesIndex < 0)
        {
            return (null, null);   // namespace does not follow the Pages folder convention
        }

        // Folders after "Pages", e.g. "Account"
        string folders = string.Join("/", parts.Skip(pagesIndex + 1));

        // "RegisterModel" -> "Register"
        string pageName = pageModelType.Name;
        if (pageName.EndsWith("Model"))
        {
            pageName = pageName.Substring(0, pageName.Length - "Model".Length);
        }

        string page = "/" + pageName;
        if (folders.Length > 0)
        {
            page = "/" + folders + "/" + pageName;
        }

        return (page, area);
    }
}
