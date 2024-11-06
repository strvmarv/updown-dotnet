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

namespace UpdownDotNetTests
{
    public class ChecksTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
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
                new() {Url = "https://i-am-a-test.com"}
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var checks = await client.Checks();

            Assert.That(() => checks.Count == 1);

            _logger.LogDebug(JsonSerializer.Serialize(checks, _jsonSerializerOptions));
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
            var check = await client.Check(mockResult.Token);

            Assert.That(() => check != null);

            _logger.LogDebug(JsonSerializer.Serialize(check, _jsonSerializerOptions));
        }

        [Test]
        public void CheckNotFound()
        {
            var mockResult = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/token")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());

            Assert.ThrowsAsync<HttpRequestException>(() => client.Check(mockResult.Token));
        }

        [Test]
        public async Task CheckCreate()
        {
            var mockParameters = new CheckParameters { Url = "https://i-am-a-test.com" };

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
        public void CheckCreateNotFound()
        {
            var mockParameters = new CheckParameters { Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<HttpRequestException>(() => client.CheckCreate(mockParameters));
        }

        [Test]
        public async Task CheckDelete()
        {
            var mockResult = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/{mockResult.Token}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new CheckDeleteResponse { Deleted = true }));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.CheckDelete(mockResult.Token);

            Assert.That(() => result.Deleted);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CheckDeleteNotFound()
        {
            var mockResult = new Check { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/{mockResult.Token}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<HttpRequestException>(() => client.CheckDelete(mockResult.Token));
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
        public void CheckUpdateNotFound()
        {
            var mockParameters = new CheckParameters { Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/token")
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());

            Assert.ThrowsAsync<HttpRequestException>(() => client.CheckUpdate("token", mockParameters));
        }
    }
}
