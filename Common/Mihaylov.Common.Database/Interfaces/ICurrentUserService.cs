namespace Mihaylov.Common;

/// <summary>
/// Provides access to the current authenticated user's identifier and username.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the identifier for the current instance.
    /// </summary>
    /// <returns>The identifier string.</returns>
    string GetId();

    /// <summary>
    /// Gets the current user's user name.
    /// </summary>
    /// <returns>The user name.</returns>
    string GetUserName();
}
