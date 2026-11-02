namespace Mihaylov.Common;

/// <summary>
/// Provides the name of the user that is recorded in the audit fields (created by, modified by).
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <returns>The user name, or <c>"System"</c> when there is no current user (startup, migrations, background work).</returns>
    string GetUserName();
}
