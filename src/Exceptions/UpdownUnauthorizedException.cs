using System;
using System.Net;

namespace UpdownDotnet.Exceptions
{
    /// <summary>
    /// Exception thrown when authentication fails (401 or 403).
    /// </summary>
    public class UpdownUnauthorizedException : UpdownApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownUnauthorizedException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code (401 or 403).</param>
        /// <param name="responseContent">The raw response content.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdownUnauthorizedException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized,
            string? responseContent = null,
            Exception? innerException = null)
            : base(message, statusCode, responseContent, innerException)
        {
        }
    }
}

