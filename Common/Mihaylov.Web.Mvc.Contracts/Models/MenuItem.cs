using System.Collections.Generic;

namespace Mihaylov.Common;

public class MenuItem(int order, string title, IMenuRoute menuRoute, string permissionPolicy, string icon, 
    IEnumerable<MenuItem> children = null, bool dividerBefore = false)
{
    public int Order { get; private set; } = order;

    public string Title { get; private set; } = title;

    public IMenuRoute MenuRoute { get; private set; } = menuRoute;

    public string PermissionPolicy { get; private set; } = permissionPolicy;

    public string Icon { get; private set; } = icon;

    public IEnumerable<MenuItem> Children { get; set; } = children;

    public bool DividerBefore { get; private set; } = dividerBefore;
}
