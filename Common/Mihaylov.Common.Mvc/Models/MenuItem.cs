using System.Collections.Generic;

namespace Mihaylov.Common;

/// <summary>
/// An item of the navigation menu.
/// </summary>
/// <param name="order">The position of the item in the menu.</param>
/// <param name="title">The text of the item.</param>
/// <param name="menuRoute">The target of the item.</param>
/// <param name="accessLevel">The access level a user needs to see the item.</param>
/// <param name="icon">The icon of the item.</param>
/// <param name="children">The sub-items of the item, or null when it has none.</param>
/// <param name="dividerBefore">True to show a divider before the item.</param>
/// <param name="isHomeMenu">True if the item is the home page of the site.</param>
public class MenuItem(int order, string title, IMenuRoute menuRoute, AccessLevel accessLevel, string icon,
    IEnumerable<MenuItem> children = null, bool dividerBefore = false, bool isHomeMenu = false)
{
    /// <summary>
    /// Gets the position of the item in the menu.
    /// </summary>
    public int Order { get; private set; } = order;

    /// <summary>
    /// Gets the text of the item.
    /// </summary>
    public string Title { get; private set; } = title;

    /// <summary>
    /// Gets the target of the item.
    /// </summary>
    public IMenuRoute MenuRoute { get; private set; } = menuRoute;

    /// <summary>
    /// Gets the access level a user needs to see the item.
    /// </summary>
    public AccessLevel AccessLevel { get; private set; } = accessLevel;

    /// <summary>
    /// Gets the icon of the item.
    /// </summary>
    public string Icon { get; private set; } = icon;

    /// <summary>
    /// Gets the sub-items of the item.
    /// </summary>
    public IEnumerable<MenuItem> Children { get; private set; } = children;

    /// <summary>
    /// Gets a value indicating whether a divider is shown before the item.
    /// </summary>
    public bool DividerBefore { get; private set; } = dividerBefore;

    /// <summary>
    /// Gets a value indicating whether the item is the home page of the site.
    /// </summary>
    public bool IsHomeMenu { get; private set; } = isHomeMenu;
}
