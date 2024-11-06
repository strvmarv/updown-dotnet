using System;
using Microsoft.Extensions.Logging;
using WireMock.Server;

namespace UpdownDotNetTests
{
    public class BaseHttpClientTest : BaseTest, IDisposable
    {
        private readonly ILogger _logger;

        protected readonly WireMockServer Server;

        public BaseHttpClientTest()
        {
            _logger = LoggerFactory.CreateLogger<BaseHttpClientTest>();
            _logger.LogDebug("Server Start");
            Server = WireMockServer.Start();
        }

        public void Dispose()
        {
            _logger.LogDebug("Server Stop");
            Server.Stop();
        }
    }
}
