using System.Net.Http;

namespace UpdownDotnet
{
    /// <summary>
    /// Client for interacting with the Updown.io monitoring API.
    /// This class provides methods for managing checks, recipients, status pages, and pulse monitoring.
    /// </summary>
    /// <remarks>
    /// The UpdownClient uses partial classes to organize API methods into logical groups.
    /// All methods are asynchronous and support cancellation tokens.
    /// </remarks>
    public partial class UpdownClient : UpdownClientBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdownClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use for API requests. 
        /// The caller is responsible for managing the HttpClient lifecycle.</param>
        public UpdownClient(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
