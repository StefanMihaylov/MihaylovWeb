using System.ComponentModel.DataAnnotations;

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
        /// Gets the timeout value, in milliseconds, for the operation.
        /// </summary>
        int? Timeout { get; }
    }

    /// <summary>
    /// Provides base configuration settings for API access.
    /// </summary>
    public class BaseApiSettings : IBaseApiSettings
    {
        /// <summary>
        /// Gets or sets the base URL for the API endpoint.
        /// </summary>
        [Required]
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timeout value in seconds.
        /// </summary>
        public int? Timeout { get; set; }
    }
}
