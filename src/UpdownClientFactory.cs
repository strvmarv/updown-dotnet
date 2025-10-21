using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UpdownDotnet
{
    /// <summary>
    /// Factory for creating <see cref="UpdownClient"/> instances.
    /// </summary>
    public class UpdownClientFactory
    {
        /// <summary>
        /// The default user agent string for HTTP requests.
        /// </summary>
        public const string UserAgentValue = "updown-dotnet";

        /// <summary>
        /// Creates a new <see cref="UpdownClient"/> instance with the specified API key.
        /// Note: This method creates a new HttpClient instance for each call. For better
        /// resource management, consider using <see cref="CreateBuilder"/> or providing
        /// your own HttpClient via <see cref="Create(HttpClient)"/>.
        /// </summary>
        /// <param name="apiKey">The Updown.io API key.</param>
        /// <returns>A configured UpdownClient instance.</returns>
        public static UpdownClient Create(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));

            return CreateBuilder()
                .WithApiKey(apiKey)
                .Build();
        }

        /// <summary>
        /// Creates a new <see cref="UpdownClient"/> instance with the provided HttpClient.
        /// The caller is responsible for managing the HttpClient lifecycle and configuration.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use for API requests.</param>
        /// <returns>A configured UpdownClient instance.</returns>
        public static UpdownClient Create(HttpClient httpClient)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            return new UpdownClient(httpClient);
        }

        /// <summary>
        /// Creates a new <see cref="UpdownClientBuilder"/> for fluent configuration.
        /// This is the recommended approach for creating clients with custom configuration.
        /// </summary>
        /// <returns>A new UpdownClientBuilder instance.</returns>
        /// <example>
        /// <code>
        /// var client = UpdownClientFactory.CreateBuilder()
        ///     .WithApiKey("your-api-key")
        ///     .WithTimeout(TimeSpan.FromSeconds(30))
        ///     .Build();
        /// </code>
        /// </example>
        public static UpdownClientBuilder CreateBuilder()
        {
            return new UpdownClientBuilder();
        }

        // Deprecated: Keep for backward compatibility
        private static HttpClient? _defaultHttpClient;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets a shared HttpClient instance. This method is deprecated and exists only
        /// for backward compatibility. Use <see cref="CreateBuilder"/> instead.
        /// </summary>
        [Obsolete("This shared HttpClient approach has thread-safety issues. Use CreateBuilder() or provide your own HttpClient instead.")]
        private static HttpClient GetOrCreateDefaultHttpClient()
        {
            if (_defaultHttpClient == null)
            {
                lock (_lock)
                {
                    if (_defaultHttpClient == null)
                    {
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

                        var httpClient = new HttpClient(handler);
                        httpClient.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("user-agent", UserAgentValue);
                        httpClient.BaseAddress = new Uri(UpdownClientBase.DefaultApiUrl);

                        _defaultHttpClient = httpClient;
                    }
                }
            }

            return _defaultHttpClient;
        }
    }
}
