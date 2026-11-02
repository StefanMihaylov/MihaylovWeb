namespace Mihaylov.Common
{
    /// <summary>
    /// Defines settings required for basic API configuration, including the API URL and optional timeout.
    /// </summary>
    public interface IBaseApiSettings
    {
        /// <summary>
        /// Gets the base URL of the API endpoint.
        /// </summary>
        string ApiUrl { get; }

        /// <summary>
        /// Gets the timeout value, in seconds, for the operation.
        /// </summary>
        int? Timeout { get; }
    }
}
