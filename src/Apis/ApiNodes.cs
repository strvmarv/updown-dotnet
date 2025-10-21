using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpdownDotnet.Models;

namespace UpdownDotnet
{
    public partial class UpdownClient
    {
        /// <summary>
        /// The API path for nodes endpoint.
        /// </summary>
        public const string NodesPath = "api/nodes";

        /// <summary>
        /// Gets all monitoring nodes/locations available in Updown.io.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of all monitoring nodes.</returns>
        /// <remarks>
        /// Returns information about all monitoring locations including their geographic
        /// coordinates, IP addresses, and location details.
        /// </remarks>
        public async Task<List<Node>> NodesAsync(CancellationToken cancellationToken = default)
        {
            var uri = new System.Uri($"{NodesPath}", System.UriKind.Relative);
            var result = await GetAsync<List<Node>>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets all IPv4 addresses used by Updown.io monitoring nodes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of IPv4 addresses.</returns>
        /// <remarks>
        /// Useful for firewall whitelisting to allow Updown.io monitoring requests.
        /// </remarks>
        public async Task<NodeIpv4Addresses> NodesIpv4Async(CancellationToken cancellationToken = default)
        {
            var uri = new System.Uri($"{NodesPath}/ipv4", System.UriKind.Relative);
            var result = await GetAsync<NodeIpv4Addresses>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets all IPv6 addresses used by Updown.io monitoring nodes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of IPv6 addresses.</returns>
        /// <remarks>
        /// Useful for firewall whitelisting to allow Updown.io monitoring requests over IPv6.
        /// </remarks>
        public async Task<NodeIpv6Addresses> NodesIpv6Async(CancellationToken cancellationToken = default)
        {
            var uri = new System.Uri($"{NodesPath}/ipv6", System.UriKind.Relative);
            var result = await GetAsync<NodeIpv6Addresses>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        // Obsolete methods for backward compatibility
        /// <summary>
        /// Gets all monitoring nodes.
        /// </summary>
        /// <returns>A list of all monitoring nodes.</returns>
        [System.Obsolete("Use NodesAsync instead.")]
        public async Task<List<Node>> Nodes()
        {
            return await NodesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all IPv4 addresses.
        /// </summary>
        /// <returns>A collection of IPv4 addresses.</returns>
        [System.Obsolete("Use NodesIpv4Async instead.")]
        public async Task<NodeIpv4Addresses> NodesIpv4()
        {
            return await NodesIpv4Async().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all IPv6 addresses.
        /// </summary>
        /// <returns>A collection of IPv6 addresses.</returns>
        [System.Obsolete("Use NodesIpv6Async instead.")]
        public async Task<NodeIpv6Addresses> NodesIpv6()
        {
            return await NodesIpv6Async().ConfigureAwait(false);
        }
    }
}

