namespace Mihaylov.Common;

/// <summary>
/// Describes the target of a menu link, either a controller action or a Razor page.
/// </summary>
public interface IMenuRoute
{
    /// <summary>
    /// Gets the area name, or an empty string when there is no area.
    /// </summary>
    string Area { get; }

    /// <summary>
    /// Gets the controller name without the <c>Controller</c> suffix. Null for a Razor page.
    /// </summary>
    string Controller { get; }

    /// <summary>
    /// Gets the action name. Null for a Razor page.
    /// </summary>
    string Action { get; }

    /// <summary>
    /// Gets the Razor page path, for example <c>/Account/Register</c>. Null for a controller action.
    /// </summary>
    string Page { get; }
}
