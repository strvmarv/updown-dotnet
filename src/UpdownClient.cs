using System.Net.Http;

namespace UpdownDotnet
{
    public partial class UpdownClient : UpdownClientBase
    {
        public UpdownClient(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
