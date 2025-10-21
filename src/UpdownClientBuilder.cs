using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UpdownDotnet
{
    /// <summary>
    /// Builder for creating configured <see cref="UpdownClient"/> instances.
    /// </summary>
    public class UpdownClientBuilder
    {
        private string? _apiKey;
        private HttpClient? _httpClient;
        private Uri? _baseAddress;
        private TimeSpan? _timeout;
        private string _userAgent = UpdownClientFactory.UserAgentValue;

        /// <summary>
        /// Sets the API key for authentication.
        /// </summary>
        /// <param name="apiKey">The Updown.io API key.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public UpdownClientBuilder WithApiKey(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            return this;
        }

        /// <summary>
        /// Sets a custom HttpClient instance. When using this option, you are responsible
        /// for configuring the HttpClient appropriately.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public UpdownClientBuilder WithHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            return this;
        }

        /// <summary>
        /// Sets the base address for the API. Defaults to https://updown.io.
        /// </summary>
        /// <param name="baseAddress">The base API address.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public UpdownClientBuilder WithBaseAddress(Uri baseAddress)
        {
            _baseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
            return this;
        }

        /// <summary>
        /// Sets the base address for the API using a string URL.
        /// </summary>
        /// <param name="baseAddress">The base API address as a string.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public UpdownClientBuilder WithBaseAddress(string baseAddress)
        {
            if (string.IsNullOrWhiteSpace(baseAddress))
                throw new ArgumentException("Base address cannot be null or empty.", nameof(baseAddress));
            
            _baseAddress = new Uri(baseAddress);
            return this;
        }

        /// <summary>
        /// Sets the HTTP request timeout.
        /// </summary>
        /// <param name="timeout">The timeout duration.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public UpdownClientBuilder WithTimeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));
            
            _timeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the user agent string for HTTP requests.
        /// </summary>
        /// <param name="userAgent">The user agent string.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public UpdownClientBuilder WithUserAgent(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                throw new ArgumentException("User agent cannot be null or empty.", nameof(userAgent));
            
            _userAgent = userAgent;
            return this;
        }

        /// <summary>
        /// Builds and returns a configured <see cref="UpdownClient"/> instance.
        /// </summary>
        /// <returns>A configured UpdownClient instance.</returns>
        public UpdownClient Build()
        {
            HttpClient httpClient;

            if (_httpClient != null)
            {
                // Use the provided HttpClient - user is responsible for configuration
                httpClient = _httpClient;
            }
            else
            {
                // Create a new HttpClient with proper configuration
#if NET5_0_OR_GREATER
                var handler = new SocketsHttpHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5)
                };
#else
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
#endif

                httpClient = new HttpClient(handler);
                
                // Set base address
                httpClient.BaseAddress = _baseAddress ?? new Uri(UpdownClientBase.DefaultApiUrl);
                
                // Set timeout if specified
                if (_timeout.HasValue)
                {
                    httpClient.Timeout = _timeout.Value;
                }

                // Configure headers
                httpClient.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("user-agent", _userAgent);

                // Add API key if provided
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    httpClient.DefaultRequestHeaders.Add(UpdownClientBase.UpdownApiKeyHeader, _apiKey);
                }
            }

            return new UpdownClient(httpClient);
        }
    }
}

