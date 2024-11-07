using System;
using Microsoft.Extensions.Logging;
using WireMock.Server;

namespace UpdownDotNetTests
{
    public class BaseHttpClientTest : BaseTest, IDisposable
    {
        private readonly ILogger _logger;

        protected readonly WireMockServer Server;
        private bool _disposedValue;

        public BaseHttpClientTest()
        {
            _logger = LoggerFactory.CreateLogger<BaseHttpClientTest>();
            _logger.LogDebug("Server Start");
            Server = WireMockServer.Start();
        }

        // ReSharper disable once FlagArgument
        protected virtual void Dispose(bool disposing)
        {
            _logger.LogDebug("Server Stop");
            Server.Stop();

            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseHttpClientTest()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
