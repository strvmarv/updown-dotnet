using System;
using System.Net;

namespace UpdownDotnet.Exceptions
{
    /// <summary>
    /// Base exception for all Updown.io API errors.
    /// </summary>
    public class UpdownApiException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with this error.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the raw response content from the API.
        /// </summary>
        public string? ResponseContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="responseContent">The raw response content.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdownApiException(
            string message,
            HttpStatusCode statusCode,
            string? responseContent = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
}

