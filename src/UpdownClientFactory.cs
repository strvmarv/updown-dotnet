using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UpdownDotnet
{
    public class UpdownClientFactory
    {
        private static readonly HttpClient DefaultHttpClient = new(new SocketsHttpHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });

        static UpdownClientFactory()
        {
            DefaultHttpClient.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
            DefaultHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
