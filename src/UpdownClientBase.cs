using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UpdownDotnet;

public class UpdownClientBase(HttpClient httpClient)
{
    public const string DefaultApiUrl = "https://updown.io";
    public const string UpdownApiKeyHeader = "X-API-KEY";

    protected HttpClient UpdownHttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    protected async Task<T> DeleteAsync<T>(Uri path)
    {
        var resp = await UpdownHttpClient.DeleteAsync(path).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        return result;
    }

    protected async Task<T> GetAsync<T>(Uri path)
    {
        var resp = await UpdownHttpClient.GetAsync(path).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        return result;
    }

    protected async Task<T> PostAsync<T>(Uri path, object content)
    {
        var resp = await UpdownHttpClient.PostAsJsonAsync(path, content, JsonOptions).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        return result;
    }

    protected async Task<T> PutAsync<T>(Uri path, object content)
    {
        var resp = await UpdownHttpClient.PutAsJsonAsync(path, content, JsonOptions).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        return result;
    }
}