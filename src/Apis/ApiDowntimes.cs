using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpdownDotnet.Models;

namespace UpdownDotnet
{
    public partial class UpdownClient
    {
        /// <summary>
        /// Gets the list of downtimes for a specific check.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="page">Optional page number for pagination (starts at 1).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of downtime periods.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        /// <remarks>
        /// Returns all downtime periods for the specified check, ordered by most recent first.
        /// Use pagination to retrieve additional results beyond the default page size.
        /// </remarks>
        public async Task<List<Downtime>> DowntimesAsync(string token, int? page = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            var uriString = page.HasValue 
                ? $"{ChecksPath}/{token}/downtimes?page={page.Value}" 
                : $"{ChecksPath}/{token}/downtimes";
            
            var uri = new Uri(uriString, UriKind.Relative);
            var result = await GetAsync<List<Downtime>>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets the list of downtimes for a specific check.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="page">Optional page number for pagination.</param>
        /// <returns>A list of downtime periods.</returns>
        [Obsolete("Use DowntimesAsync instead.")]
        public async Task<List<Downtime>> Downtimes(string token, int? page = null)
        {
            return await DowntimesAsync(token, page).ConfigureAwait(false);
        }
    }
}

