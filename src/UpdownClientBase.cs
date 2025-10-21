using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using UpdownDotnet.Exceptions;

namespace UpdownDotnet
{
    /// <summary>
    /// Base class for Updown.io API client operations.
    /// </summary>
    public class UpdownClientBase
    {
        /// <summary>
        /// The default Updown.io API base URL.
        /// </summary>
        public const string DefaultApiUrl = "https://updown.io";
        
        /// <summary>
        /// The HTTP header name for the Updown.io API key.
        /// </summary>
        public const string UpdownApiKeyHeader = "X-API-KEY";

        /// <summary>
        /// Gets the HttpClient used for API requests.
        /// </summary>
        protected readonly HttpClient UpdownHttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownClientBase"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use for API requests.</param>
        /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
        public UpdownClientBase(HttpClient httpClient)
        {
            UpdownHttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Handles HTTP response errors and throws appropriate exceptions.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="UpdownNotFoundException">Thrown when resource is not found (404).</exception>
        /// <exception cref="UpdownUnauthorizedException">Thrown when authentication fails (401/403).</exception>
        /// <exception cref="UpdownBadRequestException">Thrown when request is invalid (400).</exception>
        /// <exception cref="UpdownRateLimitException">Thrown when rate limit is exceeded (429).</exception>
        /// <exception cref="UpdownApiException">Thrown for other API errors.</exception>
        protected virtual async Task HandleErrorResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var statusCode = response.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new UpdownNotFoundException(
                        $"Resource not found: {response.RequestMessage?.RequestUri}",
                        content);

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new UpdownUnauthorizedException(
                        statusCode == HttpStatusCode.Unauthorized
                            ? "Authentication failed. Check your API key."
                            : "Access forbidden. You don't have permission to access this resource.",
                        statusCode,
                        content);

                case HttpStatusCode.BadRequest:
                    throw new UpdownBadRequestException(
                        $"Invalid request: {content}",
                        content);

                case (HttpStatusCode)429: // TooManyRequests
                    int? retryAfter = null;
                    if (response.Headers.RetryAfter?.Delta.HasValue == true)
                    {
                        retryAfter = (int)response.Headers.RetryAfter.Delta.Value.TotalSeconds;
                    }
                    throw new UpdownRateLimitException(
                        $"Rate limit exceeded. {(retryAfter.HasValue ? $"Retry after {retryAfter} seconds." : "Please try again later.")}",
                        retryAfter,
                        content);

                default:
                    throw new UpdownApiException(
                        $"API request failed with status {statusCode}: {content}",
                        statusCode,
                        content);
            }
        }

        /// <summary>
        /// Sends a DELETE request and deserializes the response.
        /// </summary>
        protected async Task<T> DeleteAsync<T>(Uri path, CancellationToken cancellationToken = default)
        {
            var resp = await UpdownHttpClient.DeleteAsync(path, cancellationToken).ConfigureAwait(false);
            
            if (!resp.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(resp, cancellationToken).ConfigureAwait(false);
            }

            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions, cancellationToken).ConfigureAwait(false);
            return result!;
        }

        /// <summary>
        /// Sends a GET request and deserializes the response.
        /// </summary>
        protected async Task<T> GetAsync<T>(Uri path, CancellationToken cancellationToken = default)
        {
            var resp = await UpdownHttpClient.GetAsync(path, cancellationToken).ConfigureAwait(false);
            
            if (!resp.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(resp, cancellationToken).ConfigureAwait(false);
            }

            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions, cancellationToken).ConfigureAwait(false);
            return result!;
        }

        /// <summary>
        /// Sends a POST request with JSON content and deserializes the response.
        /// </summary>
        protected async Task<T> PostAsync<T>(Uri path, object content, CancellationToken cancellationToken = default)
        {
            var reqContent = JsonSerializer.Serialize(content, JsonOptions);
            var resp = await UpdownHttpClient.PostAsync(path, new StringContent(reqContent, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);
            
            if (!resp.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(resp, cancellationToken).ConfigureAwait(false);
            }

            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions, cancellationToken).ConfigureAwait(false);
            return result!;
        }

        /// <summary>
        /// Sends a PUT request with JSON content and deserializes the response.
        /// </summary>
        protected async Task<T> PutAsync<T>(Uri path, object content, CancellationToken cancellationToken = default)
        {
            var reqContent = JsonSerializer.Serialize(content, JsonOptions);
            var resp = await UpdownHttpClient.PutAsync(path, new StringContent(reqContent, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);
            
            if (!resp.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(resp, cancellationToken).ConfigureAwait(false);
            }

            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions, cancellationToken).ConfigureAwait(false);
            return result!;
        }

        /// <summary>
        /// Sends a GET request to an absolute URL (used for pulse functionality).
        /// </summary>
        /// <param name="absoluteUrl">The absolute URL to request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The HTTP response message.</returns>
        protected async Task<HttpResponseMessage> HttpGetAsync(string absoluteUrl, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(absoluteUrl);
            var resp = await UpdownHttpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            
            if (!resp.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(resp, cancellationToken).ConfigureAwait(false);
            }
            
            return resp;
        }

        /// <summary>
        /// Sends a POST request to an absolute URL (used for pulse functionality).
        /// </summary>
        /// <param name="absoluteUrl">The absolute URL to request.</param>
        /// <param name="content">Optional HTTP content.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The HTTP response message.</returns>
        protected async Task<HttpResponseMessage> HttpPostAsync(string absoluteUrl, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(absoluteUrl);
            var resp = await UpdownHttpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);
            
            if (!resp.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(resp, cancellationToken).ConfigureAwait(false);
            }
            
            return resp;
        }
    }
}
