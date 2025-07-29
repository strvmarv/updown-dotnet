using System;
using System.Net.Http;
using System.Threading.Tasks;

// ReSharper disable AsyncApostle.AsyncMethodNamingHighlighting

namespace UpdownDotnet
{
    public partial class UpdownClient
    {
        /// <summary>
        /// Sends a pulse heartbeat to the specified Updown.io pulse URL using GET request.
        /// This is used for cron job and background task monitoring.
        /// </summary>
        /// <param name="pulseUrl">The complete pulse URL provided by Updown.io (e.g., "https://pulse.updown.io/token/key")</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when pulseUrl is null or empty</exception>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails</exception>
        public async Task SendPulse(string pulseUrl)
        {
            if (string.IsNullOrWhiteSpace(pulseUrl))
                throw new ArgumentNullException(nameof(pulseUrl), "Pulse URL cannot be null or empty");

            await HttpGetAsync(pulseUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a pulse heartbeat to the specified Updown.io pulse URL using POST request.
        /// This is used for cron job and background task monitoring.
        /// </summary>
        /// <param name="pulseUrl">The complete pulse URL provided by Updown.io (e.g., "https://pulse.updown.io/token/key")</param>
        /// <param name="content">Optional content to send with the POST request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when pulseUrl is null or empty</exception>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails</exception>
        public async Task SendPulsePost(string pulseUrl, HttpContent content = null)
        {
            if (string.IsNullOrWhiteSpace(pulseUrl))
                throw new ArgumentNullException(nameof(pulseUrl), "Pulse URL cannot be null or empty");

            await HttpPostAsync(pulseUrl, content).ConfigureAwait(false);
        }
    }
}