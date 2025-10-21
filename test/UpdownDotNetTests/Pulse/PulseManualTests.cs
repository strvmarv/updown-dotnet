using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UpdownDotnet;

namespace UpdownDotNetTests.Pulse
{
    public class PulseManualTests : BaseTest
    {
        // For manual testing: Set environment variables UPDOWN_API_KEY and UPDOWN_PULSE_URL
        // PowerShell: $env:UPDOWN_API_KEY="your-key"; $env:UPDOWN_PULSE_URL="https://pulse.updown.io/..."
        // Bash: export UPDOWN_API_KEY="your-key" UPDOWN_PULSE_URL="https://pulse.updown.io/..."
        // Or replace placeholders below (but don't commit!)
        private const string ApiKeyPlaceholder = "YOUR-API-KEY-HERE";
        private const string PulseUrlPlaceholder = "YOUR-PULSE-URL-HERE";
        
        private static string GetApiKey() => Environment.GetEnvironmentVariable("UPDOWN_API_KEY") ?? ApiKeyPlaceholder;
        private static string GetPulseUrl() => Environment.GetEnvironmentVariable("UPDOWN_PULSE_URL") ?? PulseUrlPlaceholder;

        private readonly ILogger _logger;

        public PulseManualTests()
        {
            _logger = LoggerFactory.CreateLogger<PulseManualTests>();
        }

        [Test, Explicit]
        public async Task SendPulse()
        {
            var apiKey = GetApiKey();
            var pulseUrl = GetPulseUrl();
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

        [Test, Explicit]
        public async Task SendPulsePost()
        {
            var apiKey = GetApiKey();
            var pulseUrl = GetPulseUrl();
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