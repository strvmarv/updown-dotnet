using System;
using System.Net;

namespace UpdownDotnet.Exceptions
{
    /// <summary>
    /// Exception thrown when API rate limit is exceeded (429).
    /// </summary>
    public class UpdownRateLimitException : UpdownApiException
    {
        /// <summary>
        /// Gets the time when the rate limit will reset, if available.
        /// </summary>
        public DateTimeOffset? ResetTime { get; }

        /// <summary>
        /// Gets the number of seconds until the rate limit resets, if available.
        /// </summary>
        public int? RetryAfterSeconds { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownRateLimitException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="retryAfterSeconds">The number of seconds until the rate limit resets.</param>
        /// <param name="responseContent">The raw response content.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdownRateLimitException(
            string message,
            int? retryAfterSeconds = null,
            string? responseContent = null,
            Exception? innerException = null)
            : base(message, (HttpStatusCode)429, responseContent, innerException)
        {
            RetryAfterSeconds = retryAfterSeconds;
            if (retryAfterSeconds.HasValue)
            {
                ResetTime = DateTimeOffset.UtcNow.AddSeconds(retryAfterSeconds.Value);
            }
        }
    }
}

