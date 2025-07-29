using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;

namespace UpdownDotNetTests.Pulse
{
    public class PulseManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";
        private const string TestPulseUrl = "YOUR-PULSE-URL-HERE";

        private readonly ILogger _logger;

        public PulseManualTests()
        {
            _logger = LoggerFactory.CreateLogger<PulseManualTests>();
        }

        [TestCase(ApiKey, TestPulseUrl), Explicit]
        public async Task SendPulse(string apiKey, string pulseUrl)
        {
            var client = UpdownClientFactory.Create(apiKey);
            
            _logger.LogInformation($"Sending pulse using URL: {pulseUrl}");
            
            try
            {
                await client.SendPulse(pulseUrl);
                _logger.LogInformation("Pulse sent successfully using URL");
                
                // If we reach here, the pulse was sent successfully
                Assert.Pass("Pulse sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send pulse using URL");
                throw;
            }
        }

        [TestCase(ApiKey, TestPulseUrl), Explicit]
        public async Task SendPulsePost(string apiKey, string pulseUrl)
        {
            var client = UpdownClientFactory.Create(apiKey);
            
            _logger.LogInformation($"Sending pulse POST using URL: {pulseUrl}");
            
            try
            {
                await client.SendPulsePost(pulseUrl);
                _logger.LogInformation("Pulse POST sent successfully using URL");
                
                // If we reach here, the pulse was sent successfully
                Assert.Pass("Pulse POST sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send pulse POST using URL");
                throw;
            }
        }
    }
}