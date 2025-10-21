using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace UpdownDotnet
{
    public partial class UpdownClient
    {
        /// <summary>
        /// Sends a pulse heartbeat to the specified Updown.io pulse URL using GET request.
        /// This is used for cron job and background task monitoring.
        /// </summary>
        /// <param name="pulseUrl">The complete pulse URL provided by Updown.io (e.g., "https://pulse.updown.io/token/key")</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when pulseUrl is null or empty.</exception>
        public async Task SendPulseAsync(string pulseUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pulseUrl))
                throw new ArgumentException("Pulse URL cannot be null or empty.", nameof(pulseUrl));

            await HttpGetAsync(pulseUrl, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a pulse heartbeat to the specified Updown.io pulse URL using POST request.
        /// This is used for cron job and background task monitoring.
        /// </summary>
        /// <param name="pulseUrl">The complete pulse URL provided by Updown.io (e.g., "https://pulse.updown.io/token/key")</param>
        /// <param name="content">Optional content to send with the POST request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when pulseUrl is null or empty.</exception>
        public async Task SendPulsePostAsync(string pulseUrl, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pulseUrl))
                throw new ArgumentException("Pulse URL cannot be null or empty.", nameof(pulseUrl));

            await HttpPostAsync(pulseUrl, content, cancellationToken).ConfigureAwait(false);
        }

        // Obsolete methods for backward compatibility
        /// <summary>
        /// Sends a pulse heartbeat using GET request.
        /// </summary>
        /// <param name="pulseUrl">The pulse URL.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Obsolete("Use SendPulseAsync instead.")]
        public async Task SendPulse(string pulseUrl)
        {
            await SendPulseAsync(pulseUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a pulse heartbeat using POST request.
        /// </summary>
        /// <param name="pulseUrl">The pulse URL.</param>
        /// <param name="content">Optional content.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Obsolete("Use SendPulsePostAsync instead.")]
        public async Task SendPulsePost(string pulseUrl, HttpContent? content = null)
        {
            await SendPulsePostAsync(pulseUrl, content).ConfigureAwait(false);
        }
    }
}
