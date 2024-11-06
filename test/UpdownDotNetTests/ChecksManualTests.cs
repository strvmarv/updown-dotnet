using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Models;

namespace UpdownDotNetTests
{
    public class ChecksManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";

        private static readonly JsonSerializerOptions JsonOptions = new()
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
            var checks = await client.Checks();

            _logger.LogDebug($"{checks.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(checks.Take(1), JsonOptions));
        }

        [TestCase(ApiKey), Explicit]
        public async Task Check(string apiKey)
        {
            var client = UpdownClientFactory.Create(apiKey);
            var checks = await client.Checks();
            var first = checks.Skip(Random.Shared.Next(0, checks.Count)).First();
            var check = await client.Check(first.Token);

            _logger.LogDebug(JsonSerializer.Serialize(check, JsonOptions));
        }

        [TestCase(ApiKey), Explicit]
        public async Task CheckCreateUpdateDelete(string apiKey)
        {
            const string url = "https://www.radancy.com";
            var client = UpdownClientFactory.Create(apiKey);

            // cleanup

            var checks = await client.Checks();
            var exists = checks.FirstOrDefault(c => url.Equals(c.Url));
            if (exists != null)
            {
                await client.CheckDelete(exists.Token);
            }
            
            // create

            var parameters = new CheckParameters
            {
                Url = url,
            };
            var check = await client.CheckCreate(parameters);

            Assert.That(check, Is.Not.Null);

            _logger.LogDebug(JsonSerializer.Serialize(check, JsonOptions));

            // update

            var updateParameters = new CheckParameters
            {
                Period = 300
            };
            var update = await client.CheckUpdate(check.Token, updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            // delete

            Assert.That(update, Is.Not.Null);
            Assert.That(update.Period, Is.EqualTo(300));
            Assert.That(check.Token, Is.EqualTo(update.Token));

            var delete = await client.CheckDelete(check.Token);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.That(delete, Is.Not.Null);
            Assert.That(delete.Deleted, Is.True);
        }
    }
}
