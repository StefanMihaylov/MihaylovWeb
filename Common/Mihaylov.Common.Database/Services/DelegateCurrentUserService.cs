using System;

namespace Mihaylov.Common;

/// <summary>
/// An <see cref="ICurrentUserService"/> that gets the name from a delegate supplied by the application.
/// </summary>
/// <param name="userNameFunc">Returns the current user name; may be <c>null</c> or return <c>null</c>.</param>
internal class DelegateCurrentUserService(Func<string> userNameFunc) : ICurrentUserService
{
    /// <inheritdoc />
    /// <remarks>Returns <c>"System"</c> when the delegate is <c>null</c> or returns <c>null</c>.</remarks>
    public string GetUserName()
    {
        const string Default = "System";

        if (userNameFunc == null)
        {
            return Default;
        }

        var result = userNameFunc() ?? Default;

        return result;
    }
}
