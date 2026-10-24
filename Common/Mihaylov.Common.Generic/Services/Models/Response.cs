using System;

namespace Mihaylov.Common
{
    /// <summary>
    /// Represents the result of an operation that yields a value of type T on success or an error message on failure.
    /// </summary>
    /// <typeparam name="T">The type of the response value returned on success.</typeparam>
    public class Response<T> where T : class
    {
        /// <summary> Gets or sets the data value. </summary>
        public T Data { get; set; }

        /// <summary> Gets or sets a value indicating whether the operation completed successfully. </summary>
        public bool IsSuccessful { get; set; }

        /// <summary> Gets or sets the error message if the operation failed; null if successful. </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new <![CDATA[Response<T>]]> instance with the specified data, marking the response as successful and
        /// omitting any error message.
        /// </summary>
        /// <param name="data">The data payload for the response.</param>
        public Response(T data) : this(data, true, null)
        {
        }

        /// <summary>
        /// Initializes a new Response with the specified error message, default data, and a failed (false) success
        /// flag.
        /// </summary>
        /// <param name="error">The error message for the response.</param>
        public Response(string error) : this(default, false, error)
        {
        }

        private Response(T data, bool isSuccessful, string errorMessage)
        {
            Data = data;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the response value when the operation succeeded.
        /// </summary>
        /// <returns>The response value of type T when the operation succeeded.</returns>
        /// <exception cref="ApplicationException">Thrown when the operation did not succeed; ErrorMessage contains failure details.</exception>
        public T GetResponse() => IsSuccessful ? Data : throw new ApplicationException(ErrorMessage);
    }
}
