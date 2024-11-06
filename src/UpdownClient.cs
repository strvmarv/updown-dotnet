using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UpdownDotnet.Models;

// ReSharper disable AsyncApostle.AsyncMethodNamingHighlighting

namespace UpdownDotnet
{
    public class UpdownClient(HttpClient httpClient) : UpdownClientBase(httpClient)
    {
        // Checks
        public const string ChecksPath = "api/checks";

        public async Task<List<Check>> Checks()
        {
            var uri = new Uri($"{ChecksPath}", UriKind.Relative);
            var result = await GetAsync<List<Check>>(uri).ConfigureAwait(false);
            return result;
        }

        public async Task<Check?> Check(string token)
        {
            var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
            var result = await GetAsync<Check>(uri).ConfigureAwait(false);
            return result;
        }

        public async Task<Check?> CheckCreate(CheckParameters parameters)
        {
            var uri = new Uri($"{ChecksPath}", UriKind.Relative);
            var result = await PostAsync<Check>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<CheckDeleteResponse> CheckDelete(string token)
        {
            var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
            var result = await DeleteAsync<CheckDeleteResponse>(uri).ConfigureAwait(false);
            return result;
        }

        public async Task<Check?> CheckUpdate(string token, CheckParameters parameters)
        {
            var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
            var result = await PutAsync<Check>(uri, parameters).ConfigureAwait(false);
            return result;
        }
    }
}
