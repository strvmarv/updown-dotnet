using System;
using System.Net;

namespace UpdownDotnet.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found (404).
    /// </summary>
    public class UpdownNotFoundException : UpdownApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="responseContent">The raw response content.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdownNotFoundException(
            string message,
            string? responseContent = null,
            Exception? innerException = null)
            : base(message, HttpStatusCode.NotFound, responseContent, innerException)
        {
        }
    }
}

