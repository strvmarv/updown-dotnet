using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Models;

namespace UpdownDotNetTests.Checks
{
    public class ChecksManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public ChecksManualTests()
        {
            _logger = LoggerFactory.CreateLogger<ChecksTests>();
        }

        [TestCase(ApiKey), Explicit]
        public async Task Checks(string apiKey)
        {
            var client = UpdownClientFactory.Create(apiKey);
            var results = await client.Checks();
            var result = results.FirstOrDefault();

            _logger.LogDebug($"{results.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(results, JsonOptions));

            Assert.That(results, Is.Not.Null);
        }

        [TestCase(ApiKey), Explicit]
        public async Task Check(string apiKey)
        {
            var client = UpdownClientFactory.Create(apiKey);
            var results = await client.Checks();
            var first = results.Skip(Random.Shared.Next(0, results.Count)).First();
            var result = await client.Check(first.Token);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Url, Is.Not.Null);
            });
        }

        [TestCase(ApiKey), Explicit]
        public async Task CheckCreateUpdateDelete(string apiKey)
        {
            var parameters = new CheckParameters
            {
                Url = "https://www.radancy.com",
                Custom_Headers = new Custom_Headers { UserAgent = "updown.io" }
            };
            var client = UpdownClientFactory.Create(apiKey);

            // cleanup

            var results = await client.Checks();
            var exists = results.FirstOrDefault(c => parameters.Url.Equals(c.Url));
            if (exists != null)
            {
                await client.CheckDelete(exists.Token);
            }

            // create
            var result = await client.CheckCreate(parameters);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Token, Is.Not.Null);
            });

            // update

            var updateParameters = new CheckParameters
            {
                Period = 300
            };
            var update = await client.CheckUpdate(result.Token, updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(update, Is.Not.Null);
                Assert.That(update.Period, Is.EqualTo(300));
                Assert.That(result.Token, Is.EqualTo(update.Token));
            });

            // delete

            var delete = await client.CheckDelete(result.Token);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(delete, Is.Not.Null);
                Assert.That(delete.Deleted, Is.True);
            });
        }
    }
}
