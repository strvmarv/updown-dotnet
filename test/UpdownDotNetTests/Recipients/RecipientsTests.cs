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

namespace UpdownDotNetTests.Recipients
{
    public class RecipientsTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public RecipientsTests()
        {
            _logger = LoggerFactory.CreateLogger<RecipientsTests>();
        }

        [Test]
        public async Task Recipients()
        {
            var mockResult = new List<Recipient>
            {
                new Recipient
                {
                    Name = "Test",
                    Type = "email",
                    Value = "stvmarv@outlook.com"
                }
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.RecipientPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var results = await client.Recipients();

            Assert.That(() => results.Count == 1);

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task RecipientCreate()
        {
            var mockParameters = new RecipientParameters
            {
                Name = "Test",
                Type = "email",
                Value = "stvmarv@outlook.com"
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.RecipientPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new Recipient(mockParameters)));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.RecipientCreate(mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void RecipientCreateNotFound()
        {
            var mockParameters = new RecipientParameters
            {
                Name = "Test",
                Type = "email",
                Value = "stvmarv@outlook.com"
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.RecipientPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<HttpRequestException>(() => client.RecipientCreate(mockParameters));
        }

        [Test]
        public async Task RecipientDelete()
        {
            var mockInput = new Recipient
            {
                Id = "Id"
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.RecipientPath}/{mockInput.Id}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { deleted = true }));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var result = await client.RecipientDelete(mockInput.Id);

            Assert.That(result?.Deleted, Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void RecipientDeleteNotFound()
        {
            var mockInput = new Recipient
            {
                Id = "Id"
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.RecipientPath}/{mockInput.Id}")
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UpdownClientFactory.Create(Server.CreateClient());
            Assert.ThrowsAsync<HttpRequestException>(() => client.RecipientDelete(mockInput.Id));
        }
    }
}
