using System;
using System.Net;

namespace UpdownDotnet.Exceptions
{
    /// <summary>
    /// Exception thrown when the API request is invalid (400).
    /// </summary>
    public class UpdownBadRequestException : UpdownApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownBadRequestException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="responseContent">The raw response content.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdownBadRequestException(
            string message,
            string? responseContent = null,
            Exception? innerException = null)
            : base(message, HttpStatusCode.BadRequest, responseContent, innerException)
        {
        }
    }
}

