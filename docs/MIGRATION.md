# Migration Guide

## Migrating from 1.x to 2.0

### Using the New Builder Pattern (Recommended)
```csharp
// Old way (still works but deprecated)
var client = UpdownClientFactory.Create("your-api-key");

// New way (recommended)
var client = new UpdownClientBuilder()
    .WithApiKey("your-api-key")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

### Using Async Methods
```csharp
// Old way (still works but deprecated)
var checks = client.Checks();

// New way (recommended)
var checks = await client.ChecksAsync();

// With cancellation token
var cts = new CancellationTokenSource();
var checks = await client.ChecksAsync(cts.Token);
```

### Using PascalCase Properties
```csharp
// Old way (still works but deprecated)
var lastStatus = check.Last_Status;

// New way (recommended)
var lastStatus = check.LastStatus;
```

### Handling Specific Exceptions
```csharp
// Old way
try
{
    var check = await client.CheckAsync("token");
}
catch (HttpRequestException ex)
{
    // Generic error handling
}

// New way
try
{
    var check = await client.CheckAsync("token");
}
catch (UpdownNotFoundException ex)
{
    // Handle 404 - check not found
}
catch (UpdownRateLimitException ex)
{
    // Handle 429 - rate limit exceeded
    await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds ?? 60));
}
catch (UpdownUnauthorizedException ex)
{
    // Handle 401 - invalid API key
}
catch (UpdownApiException ex)
{
    // Handle other API errors
}
```

### Compiler Warnings

After upgrading to 2.0, you may see `CS0618` warnings about obsolete members. These are intentional and indicate areas where you should migrate to the new API patterns. The old methods will continue to work but may be removed in a future major version.

To suppress these warnings temporarily while you migrate:
```xml
<PropertyGroup>
    <NoWarn>$(NoWarn);CS0618</NoWarn>
</PropertyGroup>
```

However, we recommend migrating your code to use the new patterns as soon as possible.
