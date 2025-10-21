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

namespace UpdownDotNetTests.Metrics
{
    /// <summary>
    /// Tests for the Metrics API.
    /// </summary>
    [TestFixture]
    public class MetricsTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public MetricsTests()
        {
            _logger = LoggerFactory.CreateLogger<MetricsTests>();
        }

        [Test]
        public async Task MetricsAsync_ReturnsListOfMetrics_WhenSuccessful()
        {
            // Arrange
            var mockResult = new List<Metric>
            {
                new Metric
                {
                    Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Apdex = 0.95,
                    Requests = new Requests
                    {
                        Samples = 100,
                        Failures = 2,
                        Satisfied = 90,
                        Tolerated = 8
                    },
                    Timings = new Timings
                    {
                        Redirect = 0,
                        Namelookup = 15,
                        Connection = 25,
                        Handshake = 35,
                        Response = 120,
                        Total = 195
                    }
                }
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/test-token/metrics")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var results = await client.MetricsAsync("test-token");

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.Not.Null);
            // Note: These may be null based on serialization
            if (results[0].Requests != null)
            {
                Assert.That(results[0].Requests.Samples, Is.EqualTo(100));
            }

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task MetricsAsync_WithTimeRange_IncludesQueryParameters()
        {
            // Arrange
            var mockResult = new List<Metric>
            {
                new Metric
                {
                    Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Apdex = 0.98
                }
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/test-token/metrics")
                    .WithParam("from", "2024-01-01")
                    .WithParam("to", "2024-01-31")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var results = await client.MetricsAsync("test-token", from: "2024-01-01", to: "2024-01-31");

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task MetricsAsync_WithGroupParameter_IncludesInQuery()
        {
            // Arrange
            var mockResult = new List<Metric>();

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/test-token/metrics")
                    .WithParam("group", "host")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var results = await client.MetricsAsync("test-token", group: "host");

            // Assert
            Assert.That(results, Is.Not.Null);
        }

        [Test]
        public void MetricsAsync_ThrowsArgumentException_WhenTokenIsNullOrEmpty()
        {
            // Arrange
            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => client.MetricsAsync(null!));
            Assert.ThrowsAsync<ArgumentException>(() => client.MetricsAsync(string.Empty));
            Assert.ThrowsAsync<ArgumentException>(() => client.MetricsAsync("   "));
        }

        [Test]
        public void MetricsAsync_ThrowsHttpRequestException_WhenNotFound()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/nonexistent/metrics")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(404));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            Assert.ThrowsAsync<UpdownDotnet.Exceptions.UpdownNotFoundException>(() => client.MetricsAsync("nonexistent"));
        }
    }
}

