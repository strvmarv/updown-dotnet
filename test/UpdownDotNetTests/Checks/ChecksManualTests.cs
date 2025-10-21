using System;
using System.Collections.Generic;
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
        // For manual testing: Set environment variable UPDOWN_API_KEY with your API key
        // PowerShell: $env:UPDOWN_API_KEY="your-key"
        // Bash: export UPDOWN_API_KEY="your-key"
        // Or replace "YOUR-API-KEY-HERE" below (but don't commit!)
        private const string ApiKeyPlaceholder = "YOUR-API-KEY-HERE";
        
        private static string GetApiKey() => Environment.GetEnvironmentVariable("UPDOWN_API_KEY") ?? ApiKeyPlaceholder;

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

        [Test, Explicit]
        public async Task Checks()
        {
            var apiKey = GetApiKey();
            var client = UpdownClientFactory.Create(apiKey);
            var results = await client.Checks();
            var result = results.FirstOrDefault();

            _logger.LogDebug($"{results.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(results, JsonOptions));

            Assert.That(results, Is.Not.Null);
        }

        [Test, Explicit]
        public async Task Check()
        {
            var apiKey = GetApiKey();
            var client = UpdownClientFactory.Create(apiKey);
            var results = await client.Checks();
            var random = new Random();
            var first = results.Skip(random.Next(0, results.Count)).First();
            var result = await client.Check(first.Token);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Url, Is.Not.Null);
            });
        }

        [Test, Explicit]
        public async Task CheckCreateUpdateDelete()
        {
            var apiKey = GetApiKey();
            var parameters = new CheckParameters
            {
                Url = "https://www.radancy.com",
                Custom_Headers = new Dictionary<string, string>()
            };
            parameters.Custom_Headers.Add("X-Test-1", "updown.io");
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
                Assert.That(result.Custom_Headers, Is.Not.Null);
                Assert.That(result.Custom_Headers.Count, Is.EqualTo(1));
            });

            // update

            var updateParameters = new CheckParameters
            {
                Period = 300,
                Custom_Headers = result.Custom_Headers
            };
            updateParameters.Custom_Headers.Add("X-Test-2", "updown.io");
            var update = await client.CheckUpdate(result.Token, updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(update, Is.Not.Null);
                Assert.That(update.Period, Is.EqualTo(300));
                Assert.That(result.Token, Is.EqualTo(update.Token));
                Assert.That(result.Custom_Headers, Is.Not.Null);
                Assert.That(result.Custom_Headers.Count, Is.EqualTo(2));
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
