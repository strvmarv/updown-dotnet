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
        /// The API path for recipients endpoint.
        /// </summary>
        public const string RecipientPath = "api/recipients";

        /// <summary>
        /// Gets all notification recipients for your account.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of all recipients.</returns>
        public async Task<List<Recipient>> RecipientsAsync(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{RecipientPath}", UriKind.Relative);
            var result = await GetAsync<List<Recipient>>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new notification recipient.
        /// </summary>
        /// <param name="parameters">The recipient parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created recipient.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        public async Task<Recipient> RecipientCreateAsync(RecipientParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var uri = new Uri($"{RecipientPath}", UriKind.Relative);
            var result = await PostAsync<Recipient>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes a notification recipient by its token.
        /// </summary>
        /// <param name="token">The recipient token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The delete response.</returns>
        /// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
        public async Task<DeleteResponse> RecipientDeleteAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            var uri = new Uri($"{RecipientPath}/{token}", UriKind.Relative);
            var result = await DeleteAsync<DeleteResponse>(uri, cancellationToken).ConfigureAwait(false);
            return result;
        }

        // Obsolete methods for backward compatibility
        /// <summary>
        /// Gets all notification recipients.
        /// </summary>
        /// <returns>A list of all recipients.</returns>
        [Obsolete("Use RecipientsAsync instead.")]
        public async Task<List<Recipient>> Recipients()
        {
            return await RecipientsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new recipient.
        /// </summary>
        /// <param name="parameters">The recipient parameters.</param>
        /// <returns>The created recipient.</returns>
        [Obsolete("Use RecipientCreateAsync instead.")]
        public async Task<Recipient> RecipientCreate(RecipientParameters parameters)
        {
            return await RecipientCreateAsync(parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a recipient.
        /// </summary>
        /// <param name="token">The recipient token.</param>
        /// <returns>The delete response.</returns>
        [Obsolete("Use RecipientDeleteAsync instead.")]
        public async Task<DeleteResponse> RecipientDelete(string token)
        {
            return await RecipientDeleteAsync(token).ConfigureAwait(false);
        }
    }
}
