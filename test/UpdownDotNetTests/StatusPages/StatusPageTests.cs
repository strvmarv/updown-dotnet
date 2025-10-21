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

namespace UpdownDotNetTests.StatusPages
{
    public class StatusPageTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public StatusPageTests()
        {
            _logger = LoggerFactory.CreateLogger<StatusPageTests>();
        }

        [Test]
        public async Task StatusPages()
        {
            var mockResult = new List<StatusPage>
            {
                new StatusPage
                {
                    Url = "https://i-am-a-test.com",
                }
            };
            
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var results = await client.StatusPages();

            Assert.That(() => results.Count == 1);

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task StatusPageCreate()
        {
            var mockParameters = new StatusPageParameters
            {
                Name = "Test",
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new StatusPage(mockParameters)));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.StatusPageCreate(mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void StatusPageCreateNotFound()
        {
            var mockParameters = new StatusPageParameters { Name = "Test" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.StatusPageCreate(mockParameters));
        }

        [Test]
        public async Task StatusPageDelete()
        {
            var mockInput = new StatusPage { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}/{mockInput.Token}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { deleted = true }));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.StatusPageDelete(mockInput.Token);

            Assert.That(result?.Deleted, Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void StatusPageDeleteNotFound()
        {
            var mockInput = new StatusPage { Token = "token", Url = "https://i-am-a-test.com" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}/{mockInput.Token}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.StatusPageDelete(mockInput.Token!));
        }

        [Test]
        public async Task StatusPageUpdate()
        {
            var mockParameters = new StatusPageParameters { Description = "Test" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}/token")
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new StatusPage(mockParameters)));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.StatusPageUpdate("token", mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void StatusPageUpdateNotFound()
        {
            var mockParameters = new StatusPageParameters { Description = "Test" };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.StatusPagesPath}/token")
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());

            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.StatusPageUpdate("token", mockParameters));
        }
    }
}
