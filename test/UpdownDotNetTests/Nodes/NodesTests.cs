using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace UpdownDotNetTests.Nodes
{
    /// <summary>
    /// Tests for the Nodes API.
    /// </summary>
    [TestFixture]
    public class NodesTests : BaseHttpClientTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public NodesTests()
        {
            _logger = LoggerFactory.CreateLogger<NodesTests>();
        }

        [Test]
        public async Task NodesAsync_ReturnsListOfNodes_WhenSuccessful()
        {
            // Arrange - Use explicit JSON string to ensure proper property name matching
            var mockJson = @"[
                {
                    ""name"": ""nyc"",
                    ""city"": ""New York"",
                    ""country"": ""United States"",
                    ""country_code"": ""US"",
                    ""lat"": 40.7128,
                    ""lng"": -74.0060,
                    ""ip"": ""1.2.3.4"",
                    ""ip6"": ""2001:db8::1""
                },
                {
                    ""name"": ""lon"",
                    ""city"": ""London"",
                    ""country"": ""United Kingdom"",
                    ""country_code"": ""GB"",
                    ""lat"": 51.5074,
                    ""lng"": -0.1278,
                    ""ip"": ""5.6.7.8"",
                    ""ip6"": ""2001:db8::2""
                }
            ]";

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.NodesPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(mockJson));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var results = await client.NodesAsync();

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results[0].Name, Is.EqualTo("nyc"));
            Assert.That(results[0].City, Is.EqualTo("New York"));
            Assert.That(results[0].CountryCode, Is.EqualTo("US"));
            Assert.That(results[1].Name, Is.EqualTo("lon"));

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task NodesIpv4Async_ReturnsIpv4Addresses_WhenSuccessful()
        {
            // Arrange - Use explicit JSON string to ensure proper property name matching
            var mockJson = @"{
                ""ipv4"": [""1.2.3.4"", ""5.6.7.8"", ""9.10.11.12""]
            }";

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.NodesPath}/ipv4")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(mockJson));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var result = await client.NodesIpv4Async();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Ipv4, Is.Not.Null);
            Assert.That(result.Ipv4!.Count, Is.EqualTo(3));
            Assert.That(result.Ipv4, Does.Contain("1.2.3.4"));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public async Task NodesIpv6Async_ReturnsIpv6Addresses_WhenSuccessful()
        {
            // Arrange - Use explicit JSON string to ensure proper property name matching
            var mockJson = @"{
                ""ipv6"": [""2001:db8::1"", ""2001:db8::2""]
            }";

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.NodesPath}/ipv6")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(mockJson));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var result = await client.NodesIpv6Async();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Ipv6, Is.Not.Null);
            Assert.That(result.Ipv6!.Count, Is.EqualTo(2));
            Assert.That(result.Ipv6, Does.Contain("2001:db8::1"));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }
    }
}

