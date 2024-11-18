using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpdownDotnet.Models;

// ReSharper disable AsyncApostle.AsyncMethodNamingHighlighting

namespace UpdownDotnet
{
    public partial class UpdownClient
    {
        public const string StatusPagesPath = "api/status_pages";

        public async Task<List<StatusPage>> StatusPages()
        {
            var uri = new Uri($"{StatusPagesPath}", UriKind.Relative);
            var result = await GetAsync<List<StatusPage>>(uri).ConfigureAwait(false);
            return result;
        }

        public async Task<StatusPage> StatusPageCreate(StatusPageParameters parameters)
        {
            var uri = new Uri($"{StatusPagesPath}", UriKind.Relative);
            var result = await PostAsync<StatusPage>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<DeleteResponse> StatusPageDelete(string token)
        {
            var uri = new Uri($"{StatusPagesPath}/{token}", UriKind.Relative);
            var result = await DeleteAsync<DeleteResponse>(uri).ConfigureAwait(false);
            return result;
        }

        public async Task<StatusPage> StatusPageUpdate(string token, StatusPageParameters parameters)
        {
            var uri = new Uri($"{StatusPagesPath}/{token}", UriKind.Relative);
            var result = await PutAsync<StatusPage>(uri, parameters).ConfigureAwait(false);
            return result;
        }
    }
}
