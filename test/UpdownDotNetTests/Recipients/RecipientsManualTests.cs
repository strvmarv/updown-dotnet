using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Models;

namespace UpdownDotNetTests.Recipients
{
    public class RecipientsManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public RecipientsManualTests()
        {
            _logger = LoggerFactory.CreateLogger<RecipientsManualTests>();
        }

        [TestCase(ApiKey), Explicit]
        public async Task Recipients(string apiKey)
        {
            var client = UpdownClientFactory.Create(apiKey);
            var results = await client.Recipients();
            var result = results.FirstOrDefault();

            _logger.LogDebug($"{results.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(results, JsonOptions));

            Assert.That(results, Is.Not.Null);
        }

        [TestCase(ApiKey), Explicit]
        public async Task RecipientCreateDelete(string apiKey)
        {
            var parameters = new RecipientParameters
            {
                Name = "Test",
                Type = "email",
                Value = "stvmarv@outlook.com"
            };
            var client = UpdownClientFactory.Create(apiKey);

            // cleanup

            var results = await client.Recipients();
            var exists = results.FirstOrDefault(c =>
                                                     parameters.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) &&
                                                     parameters.Type.Equals(c.Type, StringComparison.OrdinalIgnoreCase) &&
                                                     parameters.Value.Equals(c.Value, StringComparison.OrdinalIgnoreCase));
            if (exists != null)
            {
                await client.RecipientDelete(exists.Id);
            }

            // create


            var create = await client.RecipientCreate(parameters);

            _logger.LogDebug(JsonSerializer.Serialize(create, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(create, Is.Not.Null);
                Assert.That(create.Id, Is.Not.Null);
            });

            // delete

            var delete = await client.RecipientDelete(create.Id);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(delete, Is.Not.Null);
                Assert.That(delete.Deleted, Is.True);
            });
        }
    }
}
