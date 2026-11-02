using System.Net;
using System.Net.Http;

namespace Mihaylov.Common
{
    /// <summary>
    /// Represents an HTTP response containing the response body, HTTP status code, and a success indicator.
    /// </summary>
    public class BaseHttpResponse
    {
        /// <summary> Gets or sets the message body. </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the associated HTTP response status code is in the successful range (200-299).
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        /// <summary> Gets or sets the HTTP status code for the response. </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of BaseHttpResponse with the specified response body and HTTP response message.
        /// </summary>
        /// <param name="body">The response body as a string.</param>
        /// <param name="response">The HTTP response message used to populate the status code and success flag.</param>
        public BaseHttpResponse(string body, HttpResponseMessage response)
        {
            Body = body;
            IsSuccessStatusCode = response.IsSuccessStatusCode;
            StatusCode = response.StatusCode;
        }
    }
}
