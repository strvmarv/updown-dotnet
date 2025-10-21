# Updown.io .NET Client Architecture

## Overview

The Updown.io .NET client is a lightweight, asynchronous HTTP API client library that provides a strongly-typed interface for interacting with the Updown.io monitoring service. The library is designed with modern .NET practices, including nullable reference types, comprehensive error handling, and full async/await support.

## Design Philosophy

### 1. **Simplicity First**
The library aims to be as simple to use as possible while remaining powerful. Most common operations can be completed with just a few lines of code.

### 2. **Modern .NET Standards**
- Full support for async/await patterns
- Nullable reference types enabled
- CancellationToken support throughout
- Proper exception handling with custom exception types
- XML documentation for IntelliSense support

### 3. **Backward Compatibility**
The library maintains backward compatibility through:
- Obsolete attributes on deprecated methods
- Maintaining old property names while introducing better-named alternatives
- Careful versioning strategies

### 4. **Multi-Target Support**
Supports multiple target frameworks:
- .NET 9.0
- .NET 8.0
- .NET 6.0
- .NET Standard 2.0

## Architecture Components

### Core Classes

#### `UpdownClient`
The main entry point for all API operations. Implemented as a partial class to organize API methods into logical groups:
- `ApiChecks` - HTTP monitoring checks
- `ApiRecipients` - Notification recipients
- `ApiStatusPages` - Public status pages
- `ApiPulse` - Heartbeat monitoring
- `ApiDowntimes` - Downtime history
- `ApiMetrics` - Performance metrics
- `ApiNodes` - Monitoring node information

#### `UpdownClientBase`
Base class providing:
- HTTP request/response handling
- JSON serialization/deserialization
- Error handling and exception mapping
- Common HTTP method wrappers (GET, POST, PUT, DELETE)

#### `UpdownClientFactory`
Factory class for creating `UpdownClient` instances:
- Static methods for simple creation
- Builder pattern support for advanced configuration
- Handles HttpClient lifecycle recommendations

#### `UpdownClientBuilder`
Fluent builder for configuring clients:
- API key configuration
- Custom HttpClient injection
- Base address customization
- Timeout configuration
- User agent customization

## HttpClient Lifecycle Management

### The HttpClient Problem
HttpClient should be reused rather than created per request to avoid socket exhaustion. However, the static singleton pattern has thread-safety issues when mutating headers.

### Our Solution

#### Option 1: Provide Your Own HttpClient (Recommended for ASP.NET Core)
```csharp
// In ASP.NET Core Startup/Program.cs
services.AddHttpClient<IUpdownClient, UpdownClient>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri("https://updown.io");
    client.DefaultRequestHeaders.Add("X-API-KEY", "your-api-key");
});
```

#### Option 2: Use the Builder Pattern (Recommended for Console Apps)
```csharp
var client = UpdownClientFactory.CreateBuilder()
    .WithApiKey("your-api-key")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

#### Option 3: Simple Factory Method (Creates New HttpClient Per Call)
```csharp
var client = UpdownClientFactory.Create("your-api-key");
```

## Error Handling Strategy

### Custom Exception Hierarchy

```
Exception
└── UpdownApiException (base for all API errors)
    ├── UpdownNotFoundException (404)
    ├── UpdownUnauthorizedException (401/403)
    ├── UpdownBadRequestException (400)
    └── UpdownRateLimitException (429)
```

### Error Handling Flow

1. **HTTP Request** → Made via HttpClient
2. **Response Check** → `HandleErrorResponseAsync` examines status code
3. **Exception Creation** → Specific exception type created based on status
4. **Error Details** → Response content captured for debugging
5. **Throw** → Exception thrown to caller

### Example Error Handling

```csharp
try
{
    var check = await client.CheckAsync("token");
}
catch (UpdownNotFoundException ex)
{
    // Handle 404 - check doesn't exist
    Console.WriteLine($"Check not found: {ex.Message}");
}
catch (UpdownUnauthorizedException ex)
{
    // Handle authentication failure
    Console.WriteLine($"Auth failed: {ex.Message}");
}
catch (UpdownRateLimitException ex)
{
    // Handle rate limiting
    Console.WriteLine($"Rate limited. Retry after {ex.RetryAfterSeconds} seconds");
    if (ex.ResetTime.HasValue)
    {
        await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds ?? 60));
        // Retry request...
    }
}
catch (UpdownApiException ex)
{
    // Handle other API errors
    Console.WriteLine($"API error ({ex.StatusCode}): {ex.Message}");
    Console.WriteLine($"Response: {ex.ResponseContent}");
}
```

## Threading and Async Patterns

### Async All the Way
All API methods are fully asynchronous:
- Use `ConfigureAwait(false)` to avoid deadlocks
- Support `CancellationToken` for request cancellation
- Properly dispose resources in async methods

### Thread Safety
- `UpdownClient` instances are thread-safe for concurrent requests
- HttpClient is designed to be reused across threads
- No shared mutable state in the client

### Example with Cancellation

```csharp
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try
{
    var checks = await client.ChecksAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Request cancelled after 10 seconds");
}
```

## JSON Serialization

### Configuration
Uses `System.Text.Json` with the following options:
- `DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull`
- Ignores null values when serializing requests
- Gracefully handles missing properties when deserializing

### Property Naming
- Uses `JsonPropertyName` attributes for API field mapping
- Maintains C# naming conventions (PascalCase) in models
- Maps to API conventions (snake_case) via attributes

Example:
```csharp
[JsonPropertyName("last_check_at")]
public DateTimeOffset? LastCheckAt { get; set; }
```

## Extension Points

### Custom HttpClient Handlers
You can inject custom `HttpClientHandler` or `DelegatingHandler` instances:

```csharp
var handler = new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
    AutomaticDecompression = DecompressionMethods.All
};

var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri("https://updown.io")
};

httpClient.DefaultRequestHeaders.Add("X-API-KEY", "your-key");

var client = UpdownClientFactory.Create(httpClient);
```

### Custom Error Handling
Override `HandleErrorResponseAsync` in a derived class:

```csharp
public class CustomUpdownClient : UpdownClient
{
    public CustomUpdownClient(HttpClient httpClient) : base(httpClient) { }
    
    protected override async Task HandleErrorResponseAsync(
        HttpResponseMessage response, 
        CancellationToken cancellationToken)
    {
        // Custom error logging
        _logger.LogError($"API error: {response.StatusCode}");
        
        // Call base implementation
        await base.HandleErrorResponseAsync(response, cancellationToken);
    }
}
```

## Model Design

### Immutability vs Mutability
Models use get/set properties (mutable) for:
- Deserialization from API responses
- Easy property setting for request parameters
- Backward compatibility

Future versions may introduce record types for immutable response models.

### Nullable Reference Types
All properties are properly annotated:
- Required properties: non-nullable types
- Optional properties: nullable types
- Helps prevent null reference exceptions at compile time

### Backward Compatibility
Old property names are marked as `[Obsolete]` and delegate to new properties:

```csharp
[JsonPropertyName("last_status")]
public double? LastStatus { get; set; }

[Obsolete("Use LastStatus instead.")]
[JsonIgnore]
public double? Last_Status
{
    get => LastStatus;
    set => LastStatus = value;
}
```

## Performance Considerations

### Connection Pooling
- Uses `SocketsHttpHandler` on .NET 5+ for connection pooling
- Configures `PooledConnectionLifetime` to prevent DNS issues
- Enables automatic decompression for reduced bandwidth

### Memory Allocation
- Uses `ReadAsStreamAsync` for JSON deserialization (avoids string allocation)
- Configures JSON serializer options once and reuses
- Minimal allocations per request

### Async/Await Overhead
- Uses `ValueTask` where appropriate (future enhancement)
- Avoids synchronous blocking calls
- Properly implements async methods throughout the stack

## Testing Strategy

### Unit Tests
- Mock HTTP responses using WireMock.NET
- Test each API endpoint independently
- Verify request formatting and response parsing
- Test error handling paths

### Integration Tests
- Optional tests marked with `[Category("Integration")]`
- Run against real Updown.io API
- Require valid API key
- Should be run sparingly to avoid API rate limits

## Future Enhancements

### Planned Improvements
1. **Retry Policies** - Automatic retry with exponential backoff using Polly
2. **Rate Limiting** - Client-side rate limiting to prevent 429 errors
3. **Caching** - Optional caching layer for frequently accessed data
4. **Webhooks** - Support for receiving Updown.io webhook notifications
5. **Reactive Extensions** - IObservable-based API for real-time monitoring
6. **Source Generators** - Compile-time code generation for improved performance

### Breaking Changes in Future Versions
- Migration to record types for response models
- IAsyncEnumerable support for paginated results
- Removal of obsolete methods and properties
- Stricter nullability annotations

## Dependency Management

### Minimal Dependencies
The library has minimal external dependencies:
- `System.Text.Json` (only for .NET 6.0 and netstandard2.0)
- .NET 8.0+ includes this in the framework

### Versioning Strategy
Follows semantic versioning (SemVer):
- **MAJOR**: Breaking changes
- **MINOR**: New features, backward-compatible
- **PATCH**: Bug fixes, backward-compatible

## Security Considerations

### API Key Handling
- Never log API keys
- Store keys in secure configuration (Azure Key Vault, AWS Secrets Manager, etc.)
- Use environment variables or user secrets in development
- Rotate keys regularly

### HTTPS Only
- All communication over HTTPS
- Certificate validation enabled by default
- No support for HTTP (by design)

### Sensitive Data
- Avoid logging full API responses (may contain sensitive check URLs)
- Sanitize errors before presenting to end users
- Use structured logging with appropriate log levels

## Resources

- [Updown.io API Documentation](https://updown.io/api)
- [Microsoft HttpClient Best Practices](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)
- [Nullable Reference Types](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references)

