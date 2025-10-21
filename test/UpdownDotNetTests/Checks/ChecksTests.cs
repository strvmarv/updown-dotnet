using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace UpdownDotNetTests.Checks
{
    public class ChecksTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public ChecksTests()
        {
            _logger = LoggerFactory.CreateLogger<ChecksTests>();
        }

        [Test]
        public async Task Checks()
        {
            var mockResult = new List<Check>
            {
                new Check
                {
                    Url = "https://i-am-a-test.com",
                    Custom_Headers = new Dictionary<string, string>() { { "User-Agent", "curl" }}
                }
            };
            
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var results = await client.Checks();

            Assert.That(() => results.Count == 1);

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task Check()
        {
            var mockResult = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/token")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.Check(mockResult.Token);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CheckNotFound_ThrowsUpdownNotFoundException()
        {
            var mockInput = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/token")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());

            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.Check(mockInput.Token));
        }

        [Test]
        public async Task CheckCreate()
        {
            var mockParameters = new CheckParameters
            {
                Url = "https://i-am-a-test.com",
                Custom_Headers = new Dictionary<string, string>() { { "User-Agent", "curl" }}
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new Check(mockParameters)));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.CheckCreate(mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CheckCreateNotFound_ThrowsUpdownNotFoundException()
        {
            var mockParameters = new CheckParameters { Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.CheckCreate(mockParameters));
        }

        [Test]
        public async Task CheckDelete()
        {
            var mockInput = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/{mockInput.Token}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { deleted = true }));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.CheckDelete(mockInput.Token);

            Assert.That(result?.Deleted, Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CheckDeleteNotFound_ThrowsUpdownNotFoundException()
        {
            var mockInput = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/{mockInput.Token}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.CheckDelete(mockInput.Token));
        }

        [Test]
        public async Task CheckUpdate()
        {
            var mockParameters = new CheckParameters { Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/token")
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new Check(mockParameters)));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.CheckUpdate("token", mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CheckUpdateNotFound_ThrowsUpdownNotFoundException()
        {
            var mockParameters = new CheckParameters { Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/token")
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());

            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.CheckUpdate("token", mockParameters));
        }
    }
}
