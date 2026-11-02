using Microsoft.AspNetCore.Authorization;

namespace Mihaylov.Common
{
    /// <summary>
    /// Authorization setup shared by all hosts.
    /// </summary>
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// Registers one authorization policy per <see cref="AccessLevel"/> value that has a rule.
        /// Pass it as the configure delegate of <c>AddAuthorization</c>.
        /// </summary>
        /// <param name="options">The authorization options to add the policies to.</param>
        /// <example>
        /// <code>
        /// services.AddAuthorization(AuthorizationExtensions.GetAuthorizationOptions);
        /// </code>
        /// </example>
        public static void GetAuthorizationOptions(AuthorizationOptions options)
        {
            options.AddPolicy(AccessLevel.User.ToString(), policy => policy.RequireAuthenticatedUser());
            options.AddPolicy(AccessLevel.Admin.ToString(), policy => policy.RequireAssertion(context => context.User.IsAdmin()));
        }
    }
}
