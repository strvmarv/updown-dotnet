using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace UpdownDotNetTests.Pulse
{
    public class PulseTests : BaseHttpClientTest
    {
        private readonly ILogger _logger;

        public PulseTests()
        {
            _logger = LoggerFactory.CreateLogger<PulseTests>();
        }

        [Test]
        public async Task SendPulse_WithValidUrl_SendsGetRequest()
        {
            // Use the mock server URL instead of updown.io
            var pulseUrl = $"{Server.Url}/p/test-token";

            Server.Given(Request.Create()
                    .WithPath("/p/test-token")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("OK"));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            // This should not throw any exception
            await client.SendPulse(pulseUrl);
        }

        [Test]
        public async Task SendPulsePost_WithValidUrl_SendsPostRequest()
        {
            // Use the mock server URL instead of updown.io
            var pulseUrl = $"{Server.Url}/p/test-token";

            Server.Given(Request.Create()
                    .WithPath("/p/test-token")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("OK"));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            // This should not throw any exception
            await client.SendPulsePost(pulseUrl);
        }

        [Test]
        public void SendPulse_WithNullUrl_ThrowsArgumentException()
        {
            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            Assert.ThrowsAsync<ArgumentException>(() => client.SendPulse(null!));
        }

        [Test]
        public void SendPulse_WithEmptyUrl_ThrowsArgumentException()
        {
            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            Assert.ThrowsAsync<ArgumentException>(() => client.SendPulse(""));
        }

        [Test]
        public void SendPulse_WithWhitespaceUrl_ThrowsArgumentException()
        {
            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            Assert.ThrowsAsync<ArgumentException>(() => client.SendPulse("   "));
        }

        [Test]
        public void SendPulse_WithInvalidUrl_ThrowsUpdownNotFoundException()
        {
            // Use the mock server URL instead of updown.io
            var pulseUrl = $"{Server.Url}/p/invalid-token";

            Server.Given(Request.Create()
                    .WithPath("/p/invalid-token")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.SendPulse(pulseUrl));
        }

        [Test]
        public async Task SendPulsePost_WithCustomContent_SendsContentCorrectly()
        {
            // Use the mock server URL instead of updown.io
            var pulseUrl = $"{Server.Url}/p/test-token";
            var content = new StringContent("test data");

            Server.Given(Request.Create()
                    .WithPath("/p/test-token")
                    .UsingPost()
                    .WithBody("test data"))
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("OK"));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            
            // This should not throw any exception
            await client.SendPulsePost(pulseUrl, content);
        }
    }
}