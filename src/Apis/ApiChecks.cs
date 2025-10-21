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
        /// The API path for checks endpoint.
        /// </summary>
        public const string ChecksPath = "api/checks";

        /// <summary>
        /// Gets all checks for your account.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of all checks.</returns>
        public async Task<List<Check>> ChecksAsync(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{ChecksPath}", UriKind.Relative);
            var result = await GetAsync<List<Check>>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets a specific check by its token.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The check details.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        public async Task<Check> CheckAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
            var result = await GetAsync<Check>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new check with the specified parameters.
        /// </summary>
        /// <param name="parameters">The check parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created check.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        public async Task<Check> CheckCreateAsync(CheckParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var uri = new Uri($"{ChecksPath}", UriKind.Relative);
            var result = await PostAsync<Check>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes a check by its token.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The delete response.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        public async Task<DeleteResponse> CheckDeleteAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
            var result = await DeleteAsync<DeleteResponse>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Updates an existing check with the specified parameters.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="parameters">The check parameters to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated check.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        public async Task<Check> CheckUpdateAsync(string token, CheckParameters parameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
            var result = await PutAsync<Check>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        // Obsolete methods for backward compatibility
        /// <summary>
        /// Gets all checks for your account.
        /// </summary>
        /// <returns>A list of all checks.</returns>
        [Obsolete("Use ChecksAsync instead.")]
        public async Task<List<Check>> Checks()
        {
            return await ChecksAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a specific check by its token.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <returns>The check details.</returns>
        [Obsolete("Use CheckAsync instead.")]
        public async Task<Check> Check(string token)
        {
            return await CheckAsync(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new check.
        /// </summary>
        /// <param name="parameters">The check parameters.</param>
        /// <returns>The created check.</returns>
        [Obsolete("Use CheckCreateAsync instead.")]
        public async Task<Check> CheckCreate(CheckParameters parameters)
        {
            return await CheckCreateAsync(parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a check.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <returns>The delete response.</returns>
        [Obsolete("Use CheckDeleteAsync instead.")]
        public async Task<DeleteResponse> CheckDelete(string token)
        {
            return await CheckDeleteAsync(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates a check.
        /// </summary>
        /// <param name="token">The check token.</param>
        /// <param name="parameters">The check parameters.</param>
        /// <returns>The updated check.</returns>
        [Obsolete("Use CheckUpdateAsync instead.")]
        public async Task<Check> CheckUpdate(string token, CheckParameters parameters)
        {
            return await CheckUpdateAsync(token, parameters).ConfigureAwait(false);
        }
    }
}
