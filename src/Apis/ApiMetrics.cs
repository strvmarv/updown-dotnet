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
        /// Gets performance metrics for a specific check.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="from">Optional start time for metrics (ISO 8601 format or UNIX timestamp).</param>
        /// <param name="to">Optional end time for metrics (ISO 8601 format or UNIX timestamp).</param>
        /// <param name="group">Optional grouping interval (e.g., "time" or "host"). Default is "time".</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of metric data points.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        /// <remarks>
        /// Returns performance metrics including response times, Apdex scores, and request statistics.
        /// Metrics are aggregated over the specified time period.
        /// </remarks>
        public async Task<List<Metric>> MetricsAsync(
            string token,
            string? from = null,
            string? to = null,
            string? group = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(from))
                queryParams.Add($"from={Uri.EscapeDataString(from)}");
            if (!string.IsNullOrEmpty(to))
                queryParams.Add($"to={Uri.EscapeDataString(to)}");
            if (!string.IsNullOrEmpty(group))
                queryParams.Add($"group={Uri.EscapeDataString(group)}");

            var uriString = queryParams.Count > 0
                ? $"{ChecksPath}/{token}/metrics?{string.Join("&", queryParams)}"
                : $"{ChecksPath}/{token}/metrics";

            var uri = new Uri(uriString, UriKind.Relative);
            var result = await GetAsync<List<Metric>>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets performance metrics for a specific check.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="from">Optional start time.</param>
        /// <param name="to">Optional end time.</param>
        /// <param name="group">Optional grouping interval.</param>
        /// <returns>A list of metric data points.</returns>
        [Obsolete("Use MetricsAsync instead.")]
        public async Task<List<Metric>> Metrics(
            string token,
            string? from = null,
            string? to = null,
            string? group = null)
        {
            return await MetricsAsync(token, from, to, group).ConfigureAwait(false);
        }
    }
}

