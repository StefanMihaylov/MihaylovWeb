using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides base configuration settings for API access.
    /// </summary>
    public class BaseApiSettings : IBaseApiSettings
    {
        /// <inheritdoc />
        [Required]
        public string ApiUrl { get; set; } = string.Empty;

        /// <inheritdoc />
        public int? Timeout { get; set; }
    }
}
