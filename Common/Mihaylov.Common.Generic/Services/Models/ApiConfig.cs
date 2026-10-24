namespace Mihaylov.Common
{
    /// <summary>
    /// Configuration for connecting to an API, containing the base URL and credential fields (Username, Password, ApiKey).
    /// </summary>
    public class ApiConfig
    {
        /// <summary> Gets or sets the base URI used to construct request URIs. </summary>
        public string BaseUrl { get; set; }

        /// <summary> Gets or sets the username associated with the account. </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the password used for authentication. </summary>
        public string Password { get; set; }

        /// <summary> API key used to authenticate requests to the service. </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Initializes a new ApiConfig with the API base URL, username, password, and API key.
        /// </summary>
        /// <param name="baseUrl">Base URL of the API endpoint.</param>
        /// <param name="username">Username used for authentication.</param>
        /// <param name="password">Password used for authentication.</param>
        /// <param name="apiKey">API key used for authenticated requests.</param>
        public ApiConfig(string baseUrl, string username, string password, string apiKey)
        {
            BaseUrl = baseUrl;
            Username = username;
            Password = password;
            ApiKey = apiKey;
        }
    }
}
