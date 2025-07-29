using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UpdownDotnet
{
    public class UpdownClientBase
    {
        public const string DefaultApiUrl = "https://updown.io";
        public const string UpdownApiKeyHeader = "X-API-KEY";

        protected readonly HttpClient UpdownHttpClient;

        public UpdownClientBase(HttpClient httpClient)
        {
            UpdownHttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        protected async Task<T> DeleteAsync<T>(Uri path)
        {
            var resp = await UpdownHttpClient.DeleteAsync(path).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions).ConfigureAwait(false);
            return result;
        }

        protected async Task<T> GetAsync<T>(Uri path)
        {
            var resp = await UpdownHttpClient.GetAsync(path).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions).ConfigureAwait(false);
            return result;
        }

        protected async Task<T> PostAsync<T>(Uri path, object content)
        {
            var reqContent = JsonSerializer.Serialize(content, JsonOptions);
            var resp = await UpdownHttpClient.PostAsync(path, new StringContent(reqContent, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions).ConfigureAwait(false);
            return result;
        }

        protected async Task<T> PutAsync<T>(Uri path, object content)
        {
            var reqContent = JsonSerializer.Serialize(content, JsonOptions);
            var resp = await UpdownHttpClient.PutAsync(path, new StringContent(reqContent, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            var respContent = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(respContent, JsonOptions).ConfigureAwait(false);
            return result;
        }

        // Methods for making simple HTTP requests to absolute URLs (for pulse functionality)
        protected async Task<HttpResponseMessage> HttpGetAsync(string absoluteUrl)
        {
            var uri = new Uri(absoluteUrl);
            var resp = await UpdownHttpClient.GetAsync(uri).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            return resp;
        }

        protected async Task<HttpResponseMessage> HttpPostAsync(string absoluteUrl, HttpContent content = null)
        {
            var uri = new Uri(absoluteUrl);
            var resp = await UpdownHttpClient.PostAsync(uri, content).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            return resp;
        }
    }
}