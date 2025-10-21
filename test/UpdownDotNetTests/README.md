# UpdownDotnet Tests

This directory contains comprehensive unit and manual tests for the Updown.io .NET Client.

## Test Organization

- **Checks/** - Tests for the Checks API
- **Downtimes/** - Tests for the Downtimes API
- **ErrorHandling/** - Tests for error scenarios and exception handling
- **Metrics/** - Tests for the Metrics API
- **Nodes/** - Tests for the Nodes API
- **Pulse/** - Tests for Pulse (heartbeat) monitoring
- **Recipients/** - Tests for the Recipients API
- **StatusPages/** - Tests for the Status Pages API

## Running Tests

### Unit Tests (Default)

All automated unit tests use WireMock.Net to mock HTTP responses and do not require an API key:

```powershell
dotnet test
```

These tests run automatically in CI/CD and should always pass without any configuration.

### Manual Tests (Optional)

Manual tests are marked with `[Explicit]` and require a real Updown.io API key. They are **disabled by default** and must be explicitly run.

⚠️ **IMPORTANT: Never commit your API keys to the repository!**

#### Option 1: Command Line (Recommended)

Run specific manual tests by passing your API key as a parameter:

```powershell
# Run all manual tests for Checks
dotnet test --filter "FullyQualifiedName~ChecksManualTests"

# Run specific manual test
dotnet test --filter "FullyQualifiedName~ChecksManualTests.Checks"
```

**Note:** You'll need to temporarily edit the test file to replace `"YOUR-API-KEY-HERE"` with your actual key, then **revert the change** before committing.

#### Option 2: Using Environment Variables (Better Security)

You can modify the manual test files to read from environment variables:

```csharp
private static readonly string ApiKey = 
    Environment.GetEnvironmentVariable("UPDOWN_API_KEY") ?? "YOUR-API-KEY-HERE";
```

Then set the environment variable:

```powershell
# PowerShell
$env:UPDOWN_API_KEY="your-actual-api-key"
dotnet test --filter "FullyQualifiedName~ChecksManualTests"

# Or set permanently for your user
[System.Environment]::SetEnvironmentVariable('UPDOWN_API_KEY','your-key','User')
```

#### Option 3: Using Configuration File (Most Secure)

Create a local configuration file (already in .gitignore):

**test/UpdownDotNetTests/TestSettings.json** (this file is git-ignored):
```json
{
  "UpdownApiKey": "your-actual-api-key-here",
  "PulseUrl": "https://pulse.updown.io/YOUR-TOKEN/YOUR-KEY"
}
```

This file will never be committed thanks to `.gitignore` entries.

## Security Best Practices

### ✅ DO:
- Use placeholder strings like `"YOUR-API-KEY-HERE"` in committed code
- Use environment variables for local testing
- Use git-ignored configuration files for API keys
- Revert any changes to test files before committing
- Use separate test/development API keys (not production)

### ❌ DON'T:
- Commit real API keys to the repository
- Share API keys in pull requests or issues
- Use production API keys for testing
- Push configuration files with secrets

## Checking for Leaked Keys

Before committing, verify no keys are present:

```powershell
# Search for potential API keys (should only find placeholders)
git grep -i "api.key" test/
git grep -E "[a-z0-9]{32,}" test/ --exclude-dir=bin --exclude-dir=obj

# Check what you're about to commit
git diff --cached
```

## CI/CD Testing

Automated tests in GitHub Actions only run unit tests (with WireMock.Net). Manual tests requiring real API keys are never run in CI/CD for security reasons.

## Test Coverage

Current test coverage:
- **49 unit tests** - All passing on .NET 6, 8, and 9
- **7 manual test methods** - For optional real API testing
- **100% API endpoint coverage** - All public methods tested

## Writing New Tests

### Unit Tests (Preferred)
```csharp
[Test]
public async Task MethodName_Scenario_ExpectedOutcome()
{
    // Arrange
    Server.Given(Request.Create()
            .WithPath("/api/checks")
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithBody("[{\"token\":\"abc\"}]"));

    var client = UpdownClientFactory.Create(Server.CreateClient());

    // Act
    var result = await client.ChecksAsync();

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(1));
}
```

### Manual Tests (For Real API Validation)
```csharp
[TestCase("YOUR-API-KEY-HERE"), Explicit]
public async Task ManualTestName(string apiKey)
{
    // Test implementation using real API
}
```

## Additional Resources

- [NUnit Documentation](https://docs.nunit.org/)
- [WireMock.Net Documentation](https://github.com/WireMock-Net/WireMock.Net)
- [Updown.io API Documentation](https://updown.io/api)

