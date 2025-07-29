[![build and test](https://github.com/strvmarv/updown-dotnet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/strvmarv/updown-dotnet/actions/workflows/build-and-test.yml)

# updown-dotnet
A simple [Updown.io](https://updown.io) .NET Client

https://www.nuget.org/packages/UpdownDotnet

Don't currently utilize Updown.IO?  Join here --> https://updown.io/r/WioVu

# Notes

- This client is a simple wrapper around the Updown.io API. It does not implement all the API endpoints.
- The client uses the System.Text.Json namespace to serialize and deserialize JSON data.
- The client is asynchronous and uses the HttpClient class to make HTTP requests to the Updown.io API.
- The HttpClient is implemented per Micrsoft recommendations.  In this case, a Singleton that is reused.
- You may provide your own HttpClient instance if you want to manage the lifecycle of the HttpClient.
- Manual tests are provided if you'd like to observe the client in action.  You will need to provide your own API key.

# State

| Entity | Implemented |
---------|------------
| Checks | :white_check_mark: |
| Downtimes | :x: |
| Metrics | :x: |
| Nodes | :x: |
| Recipients | :white_check_mark: |
| Status Pages | :white_check_mark: |
| Pulse Monitoring | :white_check_mark: |

# Usage

Example usage using Checks.  Implementation across entities is similar.  Though some entities may not support all methods.  

***Use manual tests for reference.***

## Get all checks
```csharp
var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
var checks = await client.Checks();
```

## Get check by token
```csharp
var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
var check = await client.Check("EXISTING-CHECK-TOKEN");
```

## Create a check
Example: Create a check for https://your-url-here.com
```csharp
var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
var parameters = new CheckParameters
{
    Url = "https://your-url-here.com",
};
var check = await client.CheckCreate(parameters);

```

## Update a check
Example: Update the check period to 300 seconds
```csharp
var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
var updateParameters = new CheckParameters
{
    Period = 300
};
var update = await client.CheckUpdate("EXISTING-CHECK-TOKEN", updateParameters);
```

## Delete a check
```csharp
var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
var delete = await client.CheckDelete("EXISTING-CHECK-TOKEN");

```

## Pulse Monitoring (Cron/Background Job Monitoring)

Pulse monitoring is used to monitor cron jobs, background tasks, and scheduled processes. Unlike regular HTTP monitoring, pulse monitoring works by having **your application send heartbeat signals TO Updown.io**.

To use pulse monitoring:

1. Create a pulse check in your Updown.io dashboard
2. Updown.io will provide you with a unique pulse URL
3. Your application/cron job should make HTTP requests TO that URL on schedule
4. If Updown.io doesn't receive pulses within the expected timeframe, it will alert you

### Example: Sending a pulse using the client library

```csharp
var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");

// Send pulse using GET request (most common)
await client.SendPulse("https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY");

// Send pulse using POST request (if needed)
await client.SendPulsePost("https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY");
```

### Example: Manual pulse (without using the client library)
```csharp
// Simple pulse using HttpClient directly
using var httpClient = new HttpClient();
await httpClient.GetAsync("https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY");

// Or using POST
await httpClient.PostAsync("https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY", null);
```

### Example: Pulse in a cron job or scheduled task
```csharp
public async Task RunScheduledTask()
{
    var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
    
    try
    {
        // Your actual work here
        await DoImportantWork();
        
        // Send success pulse when work completes successfully
        await client.SendPulse("https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY");
    }
    catch (Exception ex)
    {
        // Handle error - Updown.io will detect missing pulse
        _logger.LogError(ex, "Scheduled task failed");
        // Don't send pulse on failure, so Updown.io alerts you
    }
}
```

### Example: Pulse with error handling
```csharp
public async Task SendHeartbeat(string pulseUrl)
{
    var client = UpdownClientFactory.Create("YOUR-API-KEY-HERE");
    
    try
    {
        await client.SendPulse(pulseUrl);
        _logger.LogInformation("Pulse sent successfully");
    }
    catch (HttpRequestException ex)
    {
        _logger.LogError(ex, "Failed to send pulse");
        // Handle pulse sending failure appropriately for your application
    }
}
```

For more information about pulse monitoring, see: https://updown.io/doc/how-pulse-cron-monitoring-works

# Contributing
Use your favorite IDE to open the project.  The project was developed using Visual Studio.

```bash
git clone https://github.com/strvmarv/updown-dotnet.git
cd updown-dotnet
dotnet restore
dotnet build
```

## Run Tests
```bash
dotnet test
