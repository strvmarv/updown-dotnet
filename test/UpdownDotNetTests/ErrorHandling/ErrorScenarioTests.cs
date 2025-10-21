using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;
using UpdownDotnet.Exceptions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace UpdownDotNetTests.ErrorHandling
{
    [TestFixture]
    public class ErrorScenarioTests : BaseHttpClientTest
    {
        private readonly ILogger _logger;

        public ErrorScenarioTests()
        {
            _logger = LoggerFactory.CreateLogger<ErrorScenarioTests>();
        }

        [Test]
        public void RateLimitExceeded_ThrowsUpdownRateLimitException_WithRetryAfter()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(429)
                    .WithHeader("Retry-After", "60")
                    .WithBody("{\"error\":\"Rate limit exceeded\"}"));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            var ex = Assert.ThrowsAsync<UpdownRateLimitException>(() => client.ChecksAsync());
            Assert.That(ex!.RetryAfterSeconds, Is.EqualTo(60));
            _logger.LogInformation($"Rate limit exception: {ex.Message}");
        }

        [Test]
        public void BadRequest_ThrowsUpdownBadRequestException()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(400)
                    .WithBody("{\"error\":\"Invalid URL format\"}"));

            var client = UpdownClientFactory.Create(Server.CreateClient());
            var parameters = new UpdownDotnet.Models.CheckParameters { Url = "invalid-url" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<UpdownBadRequestException>(() => client.CheckCreateAsync(parameters));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.ResponseContent, Does.Contain("Invalid URL"));
            _logger.LogInformation($"Bad request exception: {ex.Message}");
        }

        [Test]
        public void Unauthorized_ThrowsUpdownUnauthorizedException()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(401)
                    .WithBody("{\"error\":\"Invalid API key\"}"));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            var ex = Assert.ThrowsAsync<UpdownUnauthorizedException>(() => client.ChecksAsync());
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.ResponseContent, Does.Contain("Invalid API key"));
            _logger.LogInformation($"Unauthorized exception: {ex.Message}");
        }

        [Test]
        public void ServerError_ThrowsUpdownApiException_With500Status()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(500)
                    .WithBody("{\"error\":\"Internal server error\"}"));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            var ex = Assert.ThrowsAsync<UpdownApiException>(() => client.ChecksAsync());
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            _logger.LogInformation($"Server error exception: {ex.Message}");
        }

        [Test]
        public async Task CancellationToken_WhenCancelled_ThrowsOperationCanceledException()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("[]")
                    .WithDelay(TimeSpan.FromSeconds(10))); // Long delay to ensure cancellation

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            // TaskCanceledException is derived from OperationCanceledException
            Assert.ThrowsAsync<TaskCanceledException>(() => client.ChecksAsync(cts.Token));
        }

        [Test]
        public async Task CancellationToken_WhenNotCancelled_CompletesSuccessfully()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("[]"));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act
            var result = await client.ChecksAsync(cts.Token);

            // Assert
            Assert.That(result, Is.Not.Null);
            _logger.LogInformation("Request completed successfully with cancellation token");
        }

        [Test]
        public async Task MultipleConcurrentRequests_WithDifferentTokens_HandleCorrectly()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("[]"));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act - Fire multiple concurrent requests
            var task1 = client.ChecksAsync();
            var task2 = client.ChecksAsync();
            var task3 = client.ChecksAsync();

            await Task.WhenAll(task1, task2, task3);

            // Assert
            Assert.That(task1.Result, Is.Not.Null);
            Assert.That(task2.Result, Is.Not.Null);
            Assert.That(task3.Result, Is.Not.Null);
            _logger.LogInformation("Multiple concurrent requests completed successfully");
        }

        [Test]
        public void ServiceUnavailable_ThrowsUpdownApiException_With503Status()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(503)
                    .WithBody("{\"error\":\"Service temporarily unavailable\"}"));

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            var ex = Assert.ThrowsAsync<UpdownApiException>(() => client.ChecksAsync());
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
            _logger.LogInformation($"Service unavailable exception: {ex.Message}");
        }

        [Test]
        public void EmptyResponseBody_HandlesGracefully()
        {
            // Arrange
            Server.Given(Request.Create()
                    .WithPath($"/{UpdownClient.ChecksPath}/nonexistent")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithBody("")); // Empty response body

            var client = UpdownClientFactory.Create(Server.CreateClient());

            // Act & Assert
            var ex = Assert.ThrowsAsync<UpdownNotFoundException>(() => client.CheckAsync("nonexistent"));
            Assert.That(ex, Is.Not.Null);
            _logger.LogInformation("Empty response body handled correctly");
        }
    }
}

