using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Models;

namespace UpdownDotNetTests.StatusPages
{
    public class StatusPageManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public StatusPageManualTests()
        {
            _logger = LoggerFactory.CreateLogger<StatusPageManualTests>();
        }

        [TestCase(ApiKey), Explicit]
        public async Task StatusPages(string apiKey)
        {
            var client = UpdownClientFactory.Create(apiKey);
            var results = await client.StatusPages();
            var result = results.FirstOrDefault();

            _logger.LogDebug($"{results.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(results, JsonOptions));

            Assert.That(results, Is.Not.Null);
        }

        [TestCase(ApiKey), Explicit]
        public async Task StatusPageCreateUpdateDelete(string apiKey)
        {
            const string url = "https://i-am-a-test.com";
            var parameters = new StatusPageParameters
            {
                Name = "Test",
                Description = "Test",
                Visibility = "private",
                Checks = new List<string> { "93vf" }
            };
            var client = UpdownClientFactory.Create(apiKey);

            // cleanup

            var results = await client.StatusPages();
            var exists = results.FirstOrDefault(c => url.Equals(c.Url));
            if (exists != null)
            {
                await client.StatusPageDelete(exists.Token);
            }

            // create
            var result = await client.StatusPageCreate(parameters);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Token, Is.Not.Null);
            });

            // update

            var updateParameters = new StatusPageParameters
            {
                Description = "Updated"
            };
            var update = await client.StatusPageUpdate(result.Token, updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(update, Is.Not.Null);
                Assert.That(update.Description, Is.EqualTo("Updated"));
                Assert.That(result.Token, Is.EqualTo(update.Token));
            });

            // delete

            var delete = await client.StatusPageDelete(result.Token);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(delete, Is.Not.Null);
                Assert.That(delete.Deleted, Is.True);
            });
        }
    }
}
