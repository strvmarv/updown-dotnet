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
        /// The API path for status pages endpoint.
        /// </summary>
        public const string StatusPagesPath = "api/status_pages";

        /// <summary>
        /// Gets all status pages for your account.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of all status pages.</returns>
        public async Task<List<StatusPage>> StatusPagesAsync(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{StatusPagesPath}", UriKind.Relative);
            var result = await GetAsync<List<StatusPage>>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new status page with the specified parameters.
        /// </summary>
        /// <param name="parameters">The status page parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created status page.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        public async Task<StatusPage> StatusPageCreateAsync(StatusPageParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var uri = new Uri($"{StatusPagesPath}", UriKind.Relative);
            var result = await PostAsync<StatusPage>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes a status page by its token.
        /// </summary>
        /// <param name="token">The status page token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The delete response.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        public async Task<DeleteResponse> StatusPageDeleteAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            var uri = new Uri($"{StatusPagesPath}/{token}", UriKind.Relative);
            var result = await DeleteAsync<DeleteResponse>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Updates an existing status page with the specified parameters.
        /// </summary>
        /// <param name="token">The status page token.</param>
        /// <param name="parameters">The status page parameters to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated status page.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        public async Task<StatusPage> StatusPageUpdateAsync(string token, StatusPageParameters parameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var uri = new Uri($"{StatusPagesPath}/{token}", UriKind.Relative);
            var result = await PutAsync<StatusPage>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        // Obsolete methods for backward compatibility
        /// <summary>
        /// Gets all status pages.
        /// </summary>
        /// <returns>A list of all status pages.</returns>
        [Obsolete("Use StatusPagesAsync instead.")]
        public async Task<List<StatusPage>> StatusPages()
        {
            return await StatusPagesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new status page.
        /// </summary>
        /// <param name="parameters">The status page parameters.</param>
        /// <returns>The created status page.</returns>
        [Obsolete("Use StatusPageCreateAsync instead.")]
        public async Task<StatusPage> StatusPageCreate(StatusPageParameters parameters)
        {
            return await StatusPageCreateAsync(parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a status page.
        /// </summary>
        /// <param name="token">The status page token.</param>
        /// <returns>The delete response.</returns>
        [Obsolete("Use StatusPageDeleteAsync instead.")]
        public async Task<DeleteResponse> StatusPageDelete(string token)
        {
            return await StatusPageDeleteAsync(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates a status page.
        /// </summary>
        /// <param name="token">The status page token.</param>
        /// <param name="parameters">The status page parameters.</param>
        /// <returns>The updated status page.</returns>
        [Obsolete("Use StatusPageUpdateAsync instead.")]
        public async Task<StatusPage> StatusPageUpdate(string token, StatusPageParameters parameters)
        {
            return await StatusPageUpdateAsync(token, parameters).ConfigureAwait(false);
        }
    }
}
