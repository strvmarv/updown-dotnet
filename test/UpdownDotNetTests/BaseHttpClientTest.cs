using System;
using Microsoft.Extensions.Logging;
using WireMock.Server;

namespace UpdownDotNetTests
{
    /// <summary>
    /// Base class for tests that require HTTP mocking with WireMock.
    /// </summary>
    public class BaseHttpClientTest : BaseTest, IDisposable
    {
        private readonly ILogger _logger;
        
        /// <summary>
        /// Gets the WireMock server instance for mocking HTTP responses.
        /// </summary>
        protected readonly WireMockServer Server;
        
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHttpClientTest"/> class.
        /// </summary>
        public BaseHttpClientTest()
        {
            _logger = LoggerFactory.CreateLogger<BaseHttpClientTest>();
            _logger.LogDebug("Server Start");
            Server = WireMockServer.Start();
        }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        /// <param name="disposing">True if disposing managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _logger.LogDebug("Server Stop");
                    Server.Stop();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
