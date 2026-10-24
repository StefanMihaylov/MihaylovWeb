using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mihaylov.Common;

public class MenuRoute<TController> : IMenuRoute where TController : class
{
    public string Controller { get; }

    public string Action { get; }

    public string Area { get; }


    public MenuRoute(Expression<Func<TController, Task>> actionFunc)
    {
        var type = typeof(TController);

        Controller = GetController(type);
        Action = GetAction(actionFunc);
        Area = GetArea(type);
    }

    public MenuRoute(Expression<Action<TController>> actionFunc)
    {
        var type = typeof(TController);

        Controller = GetController(type);
        Action = GetAction(actionFunc);
        Area = GetArea(type);
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
}
