[![build and test](https://github.com/strvmarv/updown-dotnet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/strvmarv/updown-dotnet/actions/workflows/build-and-test.yml)

# updown-dotnet
A simple, modern [Updown.io](https://updown.io) .NET Client

https://www.nuget.org/packages/UpdownDotnet

Don't currently utilize Updown.IO?  Join here --> https://updown.io/r/WioVu

## Features

- ✅ **Full API Coverage**: Checks, Recipients, Status Pages, Pulse Monitoring, Downtimes, Metrics, and Nodes
- ✅ **Modern Async/Await**: All methods support async patterns with `CancellationToken`
- ✅ **Custom Exceptions**: Specific exception types for better error handling
- ✅ **Nullable Reference Types**: Full nullability annotations for safer code
- ✅ **Multi-Targeting**: Supports .NET 9, .NET 8, .NET 6, and .NET Standard 2.0
- ✅ **Comprehensive Testing**: 49+ unit tests with 100% pass rate
- ✅ **XML Documentation**: Full IntelliSense support

## Installation

```bash
dotnet add package UpdownDotnet
```

## Quick Start

```csharp
using UpdownDotnet;
using UpdownDotnet.Models;

// Create client using builder (recommended)
var client = new UpdownClientBuilder()
    .WithApiKey("your-api-key")
    .Build();

// Get all checks
var checks = await client.ChecksAsync();

// Create a new check
var checkParams = new CheckParameters
{
    Url = "https://example.com",
    Alias = "My Website",
    Period = 60 // Check every 60 seconds
};
var check = await client.CheckCreateAsync(checkParams);
```

## API Implementation Status

| Entity | Status |
|--------|--------|
| Checks | ✅ Complete |
| Downtimes | ✅ Complete |
| Metrics | ✅ Complete |
| Nodes | ✅ Complete |
| Recipients | ✅ Complete |
| Status Pages | ✅ Complete |
| Pulse Monitoring | ✅ Complete |

## Usage Examples

### Checks

```csharp
// Get all checks
var checks = await client.ChecksAsync();

// Get specific check
var check = await client.CheckAsync("check-token");

// Create a check
var parameters = new CheckParameters
{
    Url = "https://example.com",
    Alias = "Example Site",
    Period = 300,
    Enabled = true
};
var newCheck = await client.CheckCreateAsync(parameters);

// Update a check
var updateParams = new CheckParameters { Period = 600 };
var updated = await client.CheckUpdateAsync("check-token", updateParams);

// Delete a check
await client.CheckDeleteAsync("check-token");
```

### Downtimes

```csharp
// Get downtimes for a check
var downtimes = await client.DowntimesAsync("check-token");

// With pagination
var page2 = await client.DowntimesAsync("check-token", page: 2);
```

### Metrics

```csharp
// Get metrics for a check
var metrics = await client.MetricsAsync("check-token");

// Get metrics for a date range
var metrics = await client.MetricsAsync(
    "check-token",
    from: "2024-01-01",
    to: "2024-01-31"
);

// Group by time or location
var metrics = await client.MetricsAsync(
    "check-token",
    group: "time"
);
```

### Nodes

```csharp
// Get all monitoring nodes
var nodes = await client.NodesAsync();

// Get IPv4 addresses
var ipv4 = await client.NodesIpv4Async();

// Get IPv6 addresses
var ipv6 = await client.NodesIpv6Async();
```

### Pulse Monitoring (Cron/Background Jobs)

Pulse monitoring is used to monitor cron jobs, background tasks, and scheduled processes. Your application sends heartbeat signals to Updown.io on schedule.

```csharp
// Send a pulse heartbeat
await client.SendPulseAsync("https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY");

// In a scheduled job
public async Task RunScheduledTask()
{
    try
    {
        // Your work here
        await DoImportantWork();
        
        // Send success pulse
        await client.SendPulseAsync(pulseUrl);
    }
    catch (Exception ex)
    {
        // Don't send pulse on failure - Updown.io will alert you
        _logger.LogError(ex, "Task failed");
    }
}
```

## Error Handling

The client provides specific exception types for different error scenarios:

```csharp
using UpdownDotnet.Exceptions;

try
{
    var check = await client.CheckAsync("token");
}
catch (UpdownNotFoundException ex)
{
    // 404 - Check not found
    Console.WriteLine("Check doesn't exist");
}
catch (UpdownUnauthorizedException ex)
{
    // 401 - Invalid API key
    Console.WriteLine("Authentication failed");
}
catch (UpdownBadRequestException ex)
{
    // 400 - Invalid request parameters
    Console.WriteLine($"Bad request: {ex.ResponseContent}");
}
catch (UpdownRateLimitException ex)
{
    // 429 - Rate limit exceeded
    Console.WriteLine($"Rate limited. Retry after {ex.RetryAfterSeconds}s");
    
    if (ex.RetryAfterSeconds.HasValue)
    {
        await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds.Value));
        // Retry...
    }
}
catch (UpdownApiException ex)
{
    // Other API errors (500+)
    Console.WriteLine($"API error: {ex.StatusCode} - {ex.Message}");
}
```

## ASP.NET Core Integration

### Dependency Injection Setup

**Program.cs**:
```csharp
// Register with dependency injection
builder.Services.AddHttpClient<IUpdownService, UpdownService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://updown.io");
    var apiKey = builder.Configuration["Updown:ApiKey"];
    client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
});
```

**Service Implementation**:
```csharp
public class UpdownService : IUpdownService
{
    private readonly UpdownClient _client;

    public UpdownService(HttpClient httpClient)
    {
        _client = new UpdownClient(httpClient);
    }

    public async Task<List<Check>> GetAllChecksAsync(CancellationToken ct = default)
    {
        return await _client.ChecksAsync(ct);
    }
}
```

## Advanced Configuration

### Using the Builder Pattern

```csharp
var client = new UpdownClientBuilder()
    .WithApiKey("your-api-key")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .WithUserAgent("MyApp/1.0")
    .Build();
```

### Custom HttpClient

```csharp
var handler = new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
};

var client = new UpdownClientBuilder()
    .WithApiKey("your-api-key")
    .WithHttpMessageHandler(handler)
    .Build();
```

### Cancellation Token Support

```csharp
// Pass cancellation token for long-running operations
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try
{
    var checks = await client.ChecksAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Request cancelled");
}
```

## Migration from 1.x to 2.0

Version 2.0 introduces several improvements while maintaining backward compatibility:

**Old (still works, but deprecated):**
```csharp
var client = UpdownClientFactory.Create("api-key");
var checks = await client.Checks();  // No Async suffix
var check = await client.Check("token");
```

**New (recommended):**
```csharp
var client = new UpdownClientBuilder()
    .WithApiKey("api-key")
    .Build();
var checks = await client.ChecksAsync();  // Async suffix
var check = await client.CheckAsync("token");
```

For full migration details, see the [CHANGELOG](CHANGELOG.md).

## Best Practices

1. **Use the Builder Pattern**: More flexible and thread-safe than the factory
   ```csharp
   var client = new UpdownClientBuilder().WithApiKey("key").Build();
   ```

2. **Pass CancellationTokens**: Especially in web applications
   ```csharp
   await client.ChecksAsync(cancellationToken);
   ```

3. **Handle Specific Exceptions**: Use custom exception types for better error handling
   ```csharp
   catch (UpdownRateLimitException ex) { /* Handle rate limit */ }
   ```

4. **Use Async Methods**: Always use methods with `Async` suffix
   ```csharp
   await client.ChecksAsync();  // ✅ Good
   await client.Checks();       // ⚠️ Deprecated
   ```

5. **Dispose HttpClient Properly**: When using custom HttpClient instances
   ```csharp
   using var httpClient = new HttpClient();
   var client = new UpdownClient(httpClient);
   ```

## Troubleshooting

### "Unauthorized" Exception
- Verify your API key is correct
- Check that the API key is active in your Updown.io dashboard

### Rate Limiting
- Updown.io has rate limits on API calls
- Catch `UpdownRateLimitException` and respect `RetryAfterSeconds`
- Consider caching responses when appropriate

### Connection Timeouts
- Default timeout is 100 seconds
- Configure custom timeout using `UpdownClientBuilder.WithTimeout()`

## Contributing

Contributions are welcome! Please see our [Contributing Guide](docs/CONTRIBUTING.md) for details.

```bash
git clone https://github.com/strvmarv/updown-dotnet.git
cd updown-dotnet
dotnet restore
dotnet build
```

### Run Tests
```bash
dotnet test
```

## Documentation

- [API Reference](docs/API_REFERENCE.md) - Complete API documentation
- [Architecture](docs/ARCHITECTURE.md) - Design and architecture details  
- [CHANGELOG](CHANGELOG.md) - Version history and migration guides
- [Updown.io API Docs](https://updown.io/api) - Official API documentation

## License

MIT License - see [LICENSE](LICENSE) file for details

## Support

- 🐛 [Report a bug](https://github.com/strvmarv/updown-dotnet/issues)
- 💡 [Request a feature](https://github.com/strvmarv/updown-dotnet/issues)
- 📖 [View documentation](docs/)

---

Made with ❤️ for the .NET community
