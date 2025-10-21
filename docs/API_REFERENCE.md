# Updown.io .NET Client API Reference

## Table of Contents
- [Client Creation](#client-creation)
- [Checks API](#checks-api)
- [Downtimes API](#downtimes-api)
- [Metrics API](#metrics-api)
- [Nodes API](#nodes-api)
- [Recipients API](#recipients-api)
- [Status Pages API](#status-pages-api)
- [Pulse Monitoring](#pulse-monitoring)
- [Exception Types](#exception-types)
- [Models](#models)

## Client Creation

### `UpdownClientFactory.Create(string apiKey)`
Creates a simple client with the specified API key.

```csharp
var client = UpdownClientFactory.Create("your-api-key");
```

**Note**: This creates a new HttpClient instance. For production use, consider using the Builder pattern or injecting your own HttpClient.

### `UpdownClientFactory.CreateBuilder()`
Returns a fluent builder for advanced configuration.

```csharp
var client = UpdownClientFactory.CreateBuilder()
    .WithApiKey("your-api-key")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .WithUserAgent("MyApp/1.0")
    .Build();
```

### `UpdownClientFactory.Create(HttpClient httpClient)`
Creates a client using your own HttpClient instance.

```csharp
var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://updown.io")
};
httpClient.DefaultRequestHeaders.Add("X-API-KEY", "your-api-key");

var client = UpdownClientFactory.Create(httpClient);
```

## Checks API

### `ChecksAsync(CancellationToken cancellationToken = default)`
Gets all checks for your account.

**Returns**: `Task<List<Check>>`

```csharp
var checks = await client.ChecksAsync();
foreach (var check in checks)
{
    Console.WriteLine($"{check.Alias}: {check.Url} - {(check.Down == true ? "DOWN" : "UP")}");
}
```

### `CheckAsync(string token, CancellationToken cancellationToken = default)`
Gets a specific check by its token.

**Parameters**:
- `token` - The check token
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<Check>`

**Throws**:
- `ArgumentException` - When token is null or empty
- `UpdownNotFoundException` - When check doesn't exist

```csharp
var check = await client.CheckAsync("abc123");
Console.WriteLine($"Last checked: {check.LastCheckAt}");
```

### `CheckCreateAsync(CheckParameters parameters, CancellationToken cancellationToken = default)`
Creates a new check.

**Parameters**:
- `parameters` - Check configuration parameters
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<Check>`

**Throws**:
- `ArgumentNullException` - When parameters is null
- `UpdownBadRequestException` - When parameters are invalid

```csharp
var parameters = new CheckParameters
{
    Url = "https://example.com",
    Alias = "My Website",
    Period = 60, // Check every 60 seconds
    Enabled = true
};

var check = await client.CheckCreateAsync(parameters);
Console.WriteLine($"Check created with token: {check.Token}");
```

### `CheckUpdateAsync(string token, CheckParameters parameters, CancellationToken cancellationToken = default)`
Updates an existing check.

**Parameters**:
- `token` - The check token
- `parameters` - Updated check parameters
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<Check>`

```csharp
var updateParams = new CheckParameters
{
    Period = 300, // Change to 5 minutes
    Enabled = false // Disable the check
};

var updated = await client.CheckUpdateAsync("abc123", updateParams);
```

### `CheckDeleteAsync(string token, CancellationToken cancellationToken = default)`
Deletes a check.

**Parameters**:
- `token` - The check token
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<DeleteResponse>`

```csharp
var result = await client.CheckDeleteAsync("abc123");
if (result.Deleted)
{
    Console.WriteLine("Check deleted successfully");
}
```

## Downtimes API

### `DowntimesAsync(string token, int? page = null, CancellationToken cancellationToken = default)`
Gets downtime history for a specific check.

**Parameters**:
- `token` - The check token
- `page` - Optional page number for pagination
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<List<Downtime>>`

```csharp
var downtimes = await client.DowntimesAsync("abc123");
foreach (var downtime in downtimes)
{
    Console.WriteLine($"Down from {downtime.StartedAt} to {downtime.EndedAt}");
    Console.WriteLine($"Duration: {downtime.Duration} seconds");
    Console.WriteLine($"Error: {downtime.Error}");
}
```

## Metrics API

### `MetricsAsync(string token, string? from = null, string? to = null, string? group = null, CancellationToken cancellationToken = default)`
Gets performance metrics for a check.

**Parameters**:
- `token` - The check token
- `from` - Optional start time (ISO 8601 format or UNIX timestamp)
- `to` - Optional end time (ISO 8601 format or UNIX timestamp)
- `group` - Optional grouping interval ("time" or "host")
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<List<Metric>>`

```csharp
// Get metrics for the last 24 hours
var yesterday = DateTimeOffset.UtcNow.AddDays(-1).ToString("o");
var now = DateTimeOffset.UtcNow.ToString("o");

var metrics = await client.MetricsAsync("abc123", from: yesterday, to: now);
foreach (var metric in metrics)
{
    Console.WriteLine($"Apdex: {metric.Apdex}");
    Console.WriteLine($"Response time: {metric.Timings?.Total}ms");
    Console.WriteLine($"Requests: {metric.Requests?.Samples}");
}
```

## Nodes API

### `NodesAsync(CancellationToken cancellationToken = default)`
Gets all monitoring nodes/locations.

**Returns**: `Task<List<Node>>`

```csharp
var nodes = await client.NodesAsync();
foreach (var node in nodes)
{
    Console.WriteLine($"{node.Name} - {node.City}, {node.Country}");
    Console.WriteLine($"Location: {node.Latitude}, {node.Longitude}");
}
```

### `NodesIpv4Async(CancellationToken cancellationToken = default)`
Gets all IPv4 addresses used by monitoring nodes.

**Returns**: `Task<NodeIpv4Addresses>`

```csharp
var addresses = await client.NodesIpv4Async();
// Use for firewall whitelisting
foreach (var ip in addresses.Ipv4 ?? Enumerable.Empty<string>())
{
    Console.WriteLine($"Whitelist: {ip}");
}
```

### `NodesIpv6Async(CancellationToken cancellationToken = default)`
Gets all IPv6 addresses used by monitoring nodes.

**Returns**: `Task<NodeIpv6Addresses>`

```csharp
var addresses = await client.NodesIpv6Async();
foreach (var ip in addresses.Ipv6 ?? Enumerable.Empty<string>())
{
    Console.WriteLine($"Whitelist IPv6: {ip}");
}
```

## Recipients API

### `RecipientsAsync(CancellationToken cancellationToken = default)`
Gets all notification recipients.

**Returns**: `Task<List<Recipient>>`

```csharp
var recipients = await client.RecipientsAsync();
foreach (var recipient in recipients)
{
    Console.WriteLine($"{recipient.Name} ({recipient.Type}): {recipient.Value}");
}
```

### `RecipientCreateAsync(RecipientParameters parameters, CancellationToken cancellationToken = default)`
Creates a new notification recipient.

**Parameters**:
- `parameters` - Recipient configuration
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<Recipient>`

```csharp
var parameters = new RecipientParameters
{
    Type = "email",
    Name = "John Doe",
    Value = "john@example.com"
};

var recipient = await client.RecipientCreateAsync(parameters);
```

**Recipient Types**:
- `email` - Email address
- `slack` - Slack webhook URL
- `webhook` - Custom webhook URL

### `RecipientDeleteAsync(string token, CancellationToken cancellationToken = default)`
Deletes a recipient.

**Parameters**:
- `token` - The recipient ID
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<DeleteResponse>`

```csharp
await client.RecipientDeleteAsync("recipient-id");
```

## Status Pages API

### `StatusPagesAsync(CancellationToken cancellationToken = default)`
Gets all status pages.

**Returns**: `Task<List<StatusPage>>`

```csharp
var pages = await client.StatusPagesAsync();
foreach (var page in pages)
{
    Console.WriteLine($"{page.Name}: {page.Url}");
}
```

### `StatusPageCreateAsync(StatusPageParameters parameters, CancellationToken cancellationToken = default)`
Creates a new status page.

**Parameters**:
- `parameters` - Status page configuration
- `cancellationToken` - Optional cancellation token

**Returns**: `Task<StatusPage>`

```csharp
var parameters = new StatusPageParameters
{
    Name = "Service Status",
    Description = "Current status of our services",
    Visibility = "public",
    Checks = new List<string> { "check-token-1", "check-token-2" }
};

var page = await client.StatusPageCreateAsync(parameters);
```

### `StatusPageUpdateAsync(string token, StatusPageParameters parameters, CancellationToken cancellationToken = default)`
Updates a status page.

**Returns**: `Task<StatusPage>`

```csharp
var updateParams = new StatusPageParameters
{
    Name = "Updated Status Page",
    Visibility = "private",
    AccessKey = "secret-key"
};

await client.StatusPageUpdateAsync("page-token", updateParams);
```

### `StatusPageDeleteAsync(string token, CancellationToken cancellationToken = default)`
Deletes a status page.

**Returns**: `Task<DeleteResponse>`

```csharp
await client.StatusPageDeleteAsync("page-token");
```

## Pulse Monitoring

Pulse monitoring is used for cron job and background task monitoring. Your application sends heartbeats TO Updown.io.

### `SendPulseAsync(string pulseUrl, CancellationToken cancellationToken = default)`
Sends a heartbeat pulse using GET request.

**Parameters**:
- `pulseUrl` - The complete pulse URL from Updown.io
- `cancellationToken` - Optional cancellation token

**Returns**: `Task`

```csharp
// In your cron job or background task
public async Task RunScheduledTask()
{
    var client = UpdownClientFactory.Create("your-api-key");
    var pulseUrl = "https://pulse.updown.io/your-token/your-key";
    
    try
    {
        // Do your work
        await DoWork();
        
        // Send success pulse
        await client.SendPulseAsync(pulseUrl);
    }
    catch (Exception ex)
    {
        // Don't send pulse on failure - Updown.io will alert
        _logger.LogError(ex, "Task failed");
    }
}
```

### `SendPulsePostAsync(string pulseUrl, HttpContent? content = null, CancellationToken cancellationToken = default)`
Sends a heartbeat pulse using POST request with optional content.

**Parameters**:
- `pulseUrl` - The complete pulse URL from Updown.io
- `content` - Optional HTTP content to send
- `cancellationToken` - Optional cancellation token

**Returns**: `Task`

```csharp
var content = new StringContent("Task completed successfully");
await client.SendPulsePostAsync(pulseUrl, content);
```

## Exception Types

### `UpdownApiException`
Base exception for all Updown.io API errors.

**Properties**:
- `StatusCode` - HTTP status code
- `ResponseContent` - Raw response body
- `Message` - Error message

### `UpdownNotFoundException`
Thrown when a resource is not found (HTTP 404).

```csharp
try
{
    var check = await client.CheckAsync("nonexistent");
}
catch (UpdownNotFoundException ex)
{
    Console.WriteLine($"Check not found: {ex.Message}");
}
```

### `UpdownUnauthorizedException`
Thrown when authentication fails (HTTP 401 or 403).

```csharp
catch (UpdownUnauthorizedException ex)
{
    Console.WriteLine("Invalid API key or insufficient permissions");
}
```

### `UpdownBadRequestException`
Thrown when the request is invalid (HTTP 400).

```csharp
catch (UpdownBadRequestException ex)
{
    Console.WriteLine($"Invalid request: {ex.ResponseContent}");
}
```

### `UpdownRateLimitException`
Thrown when API rate limit is exceeded (HTTP 429).

**Additional Properties**:
- `RetryAfterSeconds` - Seconds until rate limit resets
- `ResetTime` - DateTimeOffset when limit resets

```csharp
catch (UpdownRateLimitException ex)
{
    Console.WriteLine($"Rate limited. Retry after {ex.RetryAfterSeconds} seconds");
    if (ex.ResetTime.HasValue)
    {
        await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds ?? 60));
        // Retry...
    }
}
```

## Models

### Check
Represents an HTTP monitoring check.

**Key Properties**:
- `Token` - Unique check identifier
- `Url` - URL being monitored
- `Alias` - Friendly name
- `Down` - Whether check is currently down
- `LastStatus` - Last HTTP status code
- `Uptime` - Uptime percentage
- `Period` - Check interval in seconds
- `Enabled` - Whether check is active
- `LastCheckAt` - Time of last check
- `NextCheckAt` - Time of next scheduled check

### CheckParameters
Parameters for creating/updating checks.

**Properties**:
- `Url` - URL to monitor
- `Alias` - Friendly name
- `Period` - Check interval (15, 30, 60, 120, 300, 600, 1800, 3600)
- `ApdexT` - Apdex threshold in seconds (0.125-8)
- `Enabled` - Enable/disable check
- `Published` - Publish on status page
- `StringMatch` - String to find in response
- `CustomHeaders` - Custom HTTP headers
- `HttpVerb` - HTTP method (GET, POST, etc.)
- `HttpBody` - Request body
- `Recipients` - List of recipient IDs to notify

### Downtime
Represents a downtime period.

**Properties**:
- `Error` - Error message
- `StartedAt` - When downtime started
- `EndedAt` - When downtime ended (null if still down)
- `Duration` - Duration in seconds

### Metric
Performance metrics for a time period.

**Properties**:
- `Time` - Timestamp (UNIX epoch)
- `Apdex` - Apdex score (0-1)
- `Requests` - Request statistics
- `Timings` - Response time breakdown

### Node
Monitoring node/location.

**Properties**:
- `Name` - Node identifier
- `City` - City name
- `Country` - Country name
- `CountryCode` - ISO country code
- `Latitude` - Geographic latitude
- `Longitude` - Geographic longitude
- `IpAddress` - IPv4 address
- `Ipv6Address` - IPv6 address

### Recipient
Notification recipient.

**Properties**:
- `Id` - Unique identifier
- `Type` - Recipient type (email, slack, webhook)
- `Name` - Display name
- `Value` - Contact value (email, URL, etc.)

### StatusPage
Public status page.

**Properties**:
- `Token` - Unique identifier
- `Url` - Public URL
- `Name` - Page name
- `Description` - Page description
- `Visibility` - public or private
- `AccessKey` - Access key for private pages
- `Checks` - List of check tokens to display

## Rate Limiting

Updown.io enforces rate limits:
- **Free plans**: 60 requests/minute
- **Paid plans**: Higher limits

When rate limited:
1. Catch `UpdownRateLimitException`
2. Check `RetryAfterSeconds` property
3. Wait before retrying
4. Consider implementing exponential backoff

## Authentication

All requests require an API key:
1. Get your API key from https://updown.io/settings/edit
2. Pass to client creation
3. Never commit API keys to source control
4. Use environment variables or secure vaults

## Best Practices

1. **Reuse HttpClient**: Don't create a new client for each request
2. **Handle Exceptions**: Wrap API calls in try-catch blocks
3. **Use Cancellation Tokens**: Support request cancellation
4. **Log Appropriately**: Don't log API keys or sensitive data
5. **Respect Rate Limits**: Implement retry logic with backoff
6. **Use Async/Await**: Don't block with `.Result` or `.Wait()`
7. **Dispose Properly**: If you create HttpClient, dispose it when done

## Examples

See the [README.md](../README.md) for comprehensive usage examples.

