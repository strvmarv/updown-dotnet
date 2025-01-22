using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UpdownDotnet
{
    public class UpdownClientFactory
    {
        public const string UserAgentValue = "updown-dotnet";

#if  NET5_0_OR_GREATER
        private static readonly HttpClient DefaultHttpClient = new HttpClient(new SocketsHttpHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
#else
        private static readonly HttpClient DefaultHttpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
#endif

        static UpdownClientFactory()
        {
            DefaultHttpClient.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
            DefaultHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultHttpClient.DefaultRequestHeaders.Add("user-agent", UserAgentValue);
            DefaultHttpClient.BaseAddress = new Uri(UpdownClientBase.DefaultApiUrl);
        }

        public static UpdownClient Create(string apiKey)
        {
            if (DefaultHttpClient.DefaultRequestHeaders.Contains(UpdownClientBase.UpdownApiKeyHeader) == false)
            {
                DefaultHttpClient.DefaultRequestHeaders.Add(UpdownClientBase.UpdownApiKeyHeader, apiKey);
            }

            return new UpdownClient(DefaultHttpClient);
        }

        public static UpdownClient Create(HttpClient httpClient)
        {
            return new UpdownClient(httpClient);
        }
    }
}
