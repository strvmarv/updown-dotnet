# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0-rc.2] - 2025-11-05

**Release Candidate 2** - This pre-release version includes dependency updates and continues to be feature-complete and ready for testing. Please report any issues before the final 2.0.0 release.

### Changed

#### Dependency Updates
- **System.Text.Json** - Updated from 9.0.6 to 9.0.10
  - Latest security patches and bug fixes
  - Improved JSON serialization performance
  
- **NUnit3TestAdapter** - Updated from 5.1.0 to 5.2.0 (test dependency)
  - Enhanced test discovery and execution
  - Better compatibility with latest .NET versions
  
- **WireMock.Net** - Updated from 1.8.0 to 1.15.0 (test dependency)
  - Improved HTTP mocking capabilities
  - Better support for modern .NET features

### Fixed

- All 147 tests pass successfully across all target frameworks (net9.0, net8.0, net6.0, netstandard2.0)

---

## [2.0.0-rc.1] - 2025-01-10

**Release Candidate** - This pre-release version is feature-complete and ready for testing. Please report any issues before the final 2.0.0 release.

This major release represents a comprehensive modernization of the Updown.io .NET Client with significant improvements to code quality, API design, documentation, and testing. While breaking changes are minimal due to backward compatibility measures, this is marked as a major version to signal the substantial enhancements and the deprecation of older patterns.

### Added

#### .NET 9 Support
- Added `net9.0` as a target framework
- Library now supports .NET 9, .NET 8, .NET 6, and .NET Standard 2.0
- All 49 tests pass on all target frameworks

#### New API Endpoints
- **Downtimes API** (`ApiDowntimes.cs`)
  - `DowntimesAsync(string token, int? page, CancellationToken)` - Retrieve downtime history for checks
  - `Downtimes(string token, int? page)` - Synchronous wrapper (marked obsolete)
  - New `Downtime` model with proper nullable annotations

- **Metrics API** (`ApiMetrics.cs`)
  - `MetricsAsync(string token, string? from, string? to, string? group, CancellationToken)` - Retrieve performance metrics
  - `Metrics(string token, string? from, string? to, string? group)` - Synchronous wrapper (marked obsolete)
  - New `Metric` and `RequestMetric` models

- **Nodes API** (`ApiNodes.cs`)
  - `NodesAsync(CancellationToken)` - List all monitoring nodes
  - `NodesIpv6Async(CancellationToken)` - List IPv6 addresses of monitoring nodes
  - `Nodes()` and `NodesIpv6()` - Synchronous wrappers (marked obsolete)
  - New `Node` model

#### API Design Improvements
- **Async Method Naming Convention**
  - All async methods now have `Async` suffix: `ChecksAsync()`, `CheckAsync()`, `CheckCreateAsync()`, etc.
  - Applied to all APIs: Checks, Recipients, StatusPages, Pulse, Downtimes, Metrics, Nodes

- **CancellationToken Support**
  - All async methods now accept optional `CancellationToken` parameter
  - Enables proper cancellation of long-running operations
  - Improves integration with modern .NET async patterns

- **UpdownClientBuilder** - New builder pattern for client configuration
  - `WithApiKey(string)` - Set API key
  - `WithBaseAddress(Uri)` - Configure custom base URL
  - `WithHttpMessageHandler(HttpMessageHandler)` - Set custom HTTP handler
  - `WithTimeout(TimeSpan)` - Configure request timeout
  - `WithUserAgent(string)` - Set custom User-Agent
  - `Build()` - Create configured `UpdownClient` instance
  - Thread-safe alternative to static `UpdownClientFactory.Create()`

#### Custom Exception Types
- `UpdownApiException` - Base exception for all API errors with `StatusCode` and `ResponseContent` properties
- `UpdownNotFoundException` - Thrown for HTTP 404 Not Found responses
- `UpdownUnauthorizedException` - Thrown for HTTP 401 Unauthorized responses
- `UpdownBadRequestException` - Thrown for HTTP 400 Bad Request responses
- `UpdownRateLimitException` - Thrown for HTTP 429 Too Many Requests with `RetryAfterSeconds` property
- Enhanced error handling in `UpdownClientBase` with specific exception types for different HTTP status codes

#### Documentation
- **XML Documentation Comments** - Complete IntelliSense support for all public APIs
  - Comprehensive parameter descriptions
  - Return value documentation
  - Exception documentation
  - Usage examples where appropriate

- **Architecture Documentation** (`docs/ARCHITECTURE.md`)
  - System overview and design philosophy
  - HttpClient lifecycle management
  - Error handling strategy
  - Threading and async patterns
  - Extension points for customization

- **API Reference Documentation** (`docs/API_REFERENCE.md`)
  - Complete API method reference
  - Model documentation
  - Authentication details
  - Rate limiting considerations
  - Code examples for all endpoints

- **Contributing Guide** (`docs/CONTRIBUTING.md`)
  - Development setup instructions
  - Testing guidelines
  - Code style conventions
  - Pull request process
  - Issue reporting guidelines

- **Enhanced README** (merged from `README_ENHANCED_EXAMPLES.md`)
  - Real-world usage scenarios
  - Error handling patterns
  - Dependency injection examples
  - ASP.NET Core integration
  - Best practices
  - Migration guide for deprecated methods
  - Troubleshooting section

#### Security Enhancements
- **Enhanced `.gitignore`** - Added comprehensive rules to prevent API key leaks
  - Protection for `*.env`, `*.env.local`, `*secrets.json` files
  - Test configuration files exclusion
  - Development/local settings protection
  
- **Security Documentation** (`SECURITY.md`)
  - Updated with comprehensive API key security best practices
  - Secure code examples (environment variables, User Secrets, Key Vault)
  - Vulnerability reporting process
  - Supported versions clearly stated
  - DO/DON'T guidelines for contributors and users
  
- **Environment Variable Template** (`.env.example`)
  - Safe template for local development configuration
  - Clear warnings about not committing secrets
  
- **Test Documentation** (`test/UpdownDotNetTests/README.md`)
  - Comprehensive testing guide
  - Security best practices for manual testing
  - Multiple options for secure API key configuration
  - Pre-commit security checks

#### Testing Improvements
- **Error Scenario Tests** (`ErrorHandling/ErrorScenarioTests.cs`)
  - Rate limit handling tests
  - Unauthorized access tests
  - Bad request handling tests
  - Server error handling tests
  - Cancellation token tests

- **Comprehensive API Tests**
  - Full test coverage for Downtimes API
  - Full test coverage for Metrics API
  - Full test coverage for Nodes API
  - Updated all existing tests to use custom exception types
  - Improved test assertions and error handling

#### Model Enhancements
- **PascalCase Properties** - All models now use C# naming conventions
  - `Check`: `LastStatus`, `LastCheckAt`, `NextCheckAt`, `CreatedAt`, `MuteUntil`, `FavIconUrl`, `CustomHeaders`, `HttpVerb`, `HttpBody`, `StringMatch`, `SslCertificate`, `DisabledLocations`, `LastDuration`, `ApdexT`, `HttpBodyMatch`
  - `Recipient`: `SelectedMonitors`, `PhoneNumber`, `SplitwiseContactId`, `MsTeamsHook`
  - `StatusPage`: `FavIconUrl`, `CustomCss`, `CustomJavascript`
  - Old snake_case properties retained with `[Obsolete]` attribute for backward compatibility

- **Nullable Reference Types** - All models properly annotated
  - Clear indication of which properties can be null
  - Compile-time null safety checks
  - Better IDE support and code analysis

- **Common Response Models** (`Models/Responses.cs`)
  - `DeleteResponse` - Standardized delete operation response

### Changed

#### Breaking Changes (Mitigated with Deprecation)
- **Language Version** - Updated to C# 9.0 (`<LangVersion>9.0</LangVersion>`) to support nullable reference types across all target frameworks
- **Nullable Reference Types** - Enabled across entire codebase (`<Nullable>enable</Nullable>`)
- **Package Version** - Updated from 1.1.0 to 2.0.0
- **Method Naming** - All async methods renamed with `Async` suffix (old methods marked obsolete)
- **Property Naming** - Model properties renamed from snake_case to PascalCase (old properties marked obsolete)

#### Improvements
- **HttpClient Configuration**
  - Improved `UpdownClientFactory` thread-safety
  - Added `PropertyNameCaseInsensitive = true` to JSON serialization options for better flexibility
  - Better connection pooling with `SocketsHttpHandler` (NET5.0+)
  - Automatic decompression support (GZip, Deflate)

- **Error Handling**
  - Enhanced error messages with context
  - Specific exceptions for different error types
  - Rate limit information included in exceptions
  - Better handling of null responses

- **Code Quality**
  - Consistent use of `ConfigureAwait(false)` for library code
  - Proper parameter validation with meaningful error messages
  - Better separation of concerns with partial classes
  - Improved null handling throughout

### Deprecated

- **UpdownClientFactory Methods**
  - `Create(string apiKey)` - Use `UpdownClientBuilder` instead for better thread-safety
  - `Create(HttpClient httpClient)` - Use `UpdownClientBuilder` instead for more flexible configuration

- **API Methods (All deprecated in favor of Async versions)**
  - **Checks API**: `Checks()`, `Check()`, `CheckCreate()`, `CheckUpdate()`, `CheckDelete()`
  - **Recipients API**: `Recipients()`, `Recipient()`, `RecipientCreate()`, `RecipientUpdate()`, `RecipientDelete()`
  - **StatusPages API**: `StatusPages()`, `StatusPage()`, `StatusPageCreate()`, `StatusPageUpdate()`, `StatusPageDelete()`
  - **Pulse API**: `SendPulse()`
  - **Downtimes API**: `Downtimes()`
  - **Metrics API**: `Metrics()`
  - **Nodes API**: `Nodes()`, `NodesIpv6()`

- **Model Properties (All deprecated in favor of PascalCase versions)**
  - `Check`: `Last_Status`, `Last_Check_At`, `Next_Check_At`, `Created_At`, `Mute_Until`, `Favicon_Url`, `Custom_Headers`, `Http_Verb`, `Http_Body`, `String_Match`, `Ssl`, `Disabled_Locations`, `Last_Duration`, `Apdex_T`, `Down_Since`
  - `Recipient`: `Selected_Monitors`, `Phone_Number`, `Splitwise_Contact_Id`, `Msteams_Hook`
  - `StatusPage`: `Favicon_Url`, `Custom_Css`, `Custom_Javascript`
  - `Downtime`: `Started_At`, `Ended_At`
  - `Metric`: `Apdex_T`, `Error_Rate`, `Response_Time`

### Fixed

- **netstandard2.0 Compatibility**
  - Replaced `HttpStatusCode.TooManyRequests` with `(HttpStatusCode)429` for netstandard2.0 compatibility
  - Added explicit C# language version to support nullable reference types in netstandard2.0

- **Test Reliability**
  - Fixed test failures related to null reference handling
  - Updated test assertions to use new custom exception types
  - Improved mock data to correctly serialize/deserialize all properties
  - Added null checks in tests to prevent false positives

- **Serialization Issues**
  - Fixed JSON property name mapping with `[JsonPropertyName]` attributes
  - Ensured proper deserialization of all model properties
  - Added case-insensitive property matching for robustness

### Security

- Enhanced error handling to avoid leaking sensitive information
- Improved validation of user inputs to prevent injection attacks
- Better handling of API authentication errors
- **Manual Test Security**: All manual test files now read API keys from environment variables (`UPDOWN_API_KEY`, `UPDOWN_PULSE_URL`) instead of requiring hardcoded values, reducing risk of accidental credential commits

### Performance

- Optimized HttpClient usage with connection pooling
- Reduced memory allocations with better async patterns
- Improved JSON serialization performance with System.Text.Json

---

## [1.1.0] - Previous Release

### Features
- Initial implementation of Checks API
- Initial implementation of Recipients API
- Initial implementation of StatusPages API
- Initial implementation of Pulse Monitoring
- Basic error handling with HttpClient

### Target Frameworks
- .NET 8.0
- .NET 6.0
- .NET Standard 2.0

---

## Migration Guide

### Migrating from 1.x to 2.0

#### Using the New Builder Pattern (Recommended)
```csharp
// Old way (still works but deprecated)
var client = UpdownClientFactory.Create("your-api-key");

// New way (recommended)
var client = new UpdownClientBuilder()
    .WithApiKey("your-api-key")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

#### Using Async Methods
```csharp
// Old way (still works but deprecated)
var checks = client.Checks();

// New way (recommended)
var checks = await client.ChecksAsync();

// With cancellation token
var cts = new CancellationTokenSource();
var checks = await client.ChecksAsync(cts.Token);
```

#### Using PascalCase Properties
```csharp
// Old way (still works but deprecated)
var lastStatus = check.Last_Status;

// New way (recommended)
var lastStatus = check.LastStatus;
```

#### Handling Specific Exceptions
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

---

## Support

For questions, issues, or contributions, please visit:
- **GitHub Repository**: https://github.com/strvmarv/updown-dotnet
- **Issue Tracker**: https://github.com/strvmarv/updown-dotnet/issues
- **Documentation**: See `/docs` folder for detailed documentation

---

[2.0.0-rc.2]: https://github.com/strvmarv/updown-dotnet/compare/v2.0.0-rc.1...v2.0.0-rc.2
[2.0.0-rc.1]: https://github.com/strvmarv/updown-dotnet/compare/v1.1.0...v2.0.0-rc.1
[1.1.0]: https://github.com/strvmarv/updown-dotnet/releases/tag/v1.1.0

