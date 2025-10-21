using System;
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

namespace UpdownDotNetTests.Downtimes
{
    /// <summary>
    /// Tests for the Downtimes API.
    /// </summary>
    [TestFixture]
    public class DowntimesTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public DowntimesTests()
        {
            _logger = LoggerFactory.CreateLogger<DowntimesTests>();
        }

        [Test]
        public async Task DowntimesAsync_ReturnsListOfDowntimes_WhenSuccessful()
        {
            // Arrange - Use JSON string to ensure proper property name matching
            var mockJson = @"[{
                ""error"": ""Connection timeout"",
                ""started_at"": ""2023-01-01T10:00:00Z"",
                ""ended_at"": ""2023-01-01T11:00:00Z"",
                ""duration"": 3600
            }]";

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/test-token/downtimes")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(mockJson));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var results = await client.DowntimesAsync("test-token");

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.Not.Null);
            Assert.That(results[0].Duration, Is.EqualTo(3600));

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task DowntimesAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange - Use JSON string to ensure proper property name matching
            var mockJson = @"[{
                ""error"": ""Page 2 downtime"",
                ""started_at"": ""2023-01-01T06:00:00Z"",
                ""ended_at"": ""2023-01-01T07:00:00Z"",
                ""duration"": 3600
            }]";

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/test-token/downtimes")
                    .WithParam("page", "2")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(mockJson));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var results = await client.DowntimesAsync("test-token", page: 2);

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.Not.Null);
        }

        [Test]
        public void DowntimesAsync_ThrowsArgumentException_WhenTokenIsNullOrEmpty()
        {
            // Arrange
            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => client.DowntimesAsync(null!));
            Assert.ThrowsAsync<ArgumentException>(() => client.DowntimesAsync(string.Empty));
            Assert.ThrowsAsync<ArgumentException>(() => client.DowntimesAsync("   "));
        }

        [Test]
        public void DowntimesAsync_ThrowsHttpRequestException_WhenNotFound()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/nonexistent/downtimes")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(404));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.DowntimesAsync("nonexistent"));
        }
    }
}

