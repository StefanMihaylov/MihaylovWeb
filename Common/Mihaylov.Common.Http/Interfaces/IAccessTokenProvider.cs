namespace Mihaylov.Common
{
    /// <summary>
    /// Provides functionality to retrieve access tokens for authentication purposes.
    /// </summary>
    public interface IAccessTokenProvider
    {
        /// <summary>
        /// The name of the cookie used to store the access token.
        /// </summary>
        public const string COOKIE_NAME = "X-Access-Token";

        /// <summary>
        /// Retrieves the authentication token as a string.
        /// </summary>
        /// <returns>A string representing the authentication token.</returns>
        string GetToken();
    }
}
