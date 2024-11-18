using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpdownDotnet.Models;

// ReSharper disable AsyncApostle.AsyncMethodNamingHighlighting

namespace UpdownDotnet
{
    public partial class UpdownClient
    {
        public const string RecipientPath = "api/recipients";

        public async Task<List<Recipient>> Recipients()
        {
            var uri = new Uri($"{RecipientPath}", UriKind.Relative);
            var result = await GetAsync<List<Recipient>>(uri).ConfigureAwait(false);
            return result;
        }

        public async Task<Recipient> RecipientCreate(RecipientParameters parameters)
        {
            var uri = new Uri($"{RecipientPath}", UriKind.Relative);
            var result = await PostAsync<Recipient>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<DeleteResponse> RecipientDelete(string token)
        {
            var uri = new Uri($"{RecipientPath}/{token}", UriKind.Relative);
            var result = await DeleteAsync<DeleteResponse>(uri).ConfigureAwait(false);
            return result;
        }
    }
}
