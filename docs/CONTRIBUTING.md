# Contributing to Updown.io .NET Client

Thank you for considering contributing to the Updown.io .NET Client! This document provides guidelines and instructions for contributing to the project.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [Project Structure](#project-structure)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Pull Request Process](#pull-request-process)
- [Release Process](#release-process)

## Code of Conduct

This project adheres to a code of conduct that all contributors are expected to follow:

- Be respectful and inclusive
- Welcome newcomers and help them learn
- Focus on what is best for the community
- Show empathy towards other community members
- Be patient with questions and different perspectives

## Getting Started

### Prerequisites
- .NET SDK 6.0 or later
- Git
- A code editor (Visual Studio, VS Code, or Rider recommended)
- An Updown.io account (for integration testing)

### Fork and Clone

1. Fork the repository on GitHub
2. Clone your fork locally:
```bash
git clone https://github.com/YOUR-USERNAME/updown-dotnet.git
cd updown-dotnet
```

3. Add the upstream repository:
```bash
git remote add upstream https://github.com/strvmarv/updown-dotnet.git
```

## Development Setup

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for specific framework
dotnet test --framework net8.0

# Run with detailed output
dotnet test --verbosity detailed

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

### Integration Tests

Integration tests are marked with `[Category("Integration")]` and require a valid API key:

1. Set your API key in environment variable:
```bash
# Windows PowerShell
$env:UPDOWN_API_KEY = "your-api-key"

# Linux/Mac
export UPDOWN_API_KEY="your-api-key"
```

2. Run integration tests:
```bash
dotnet test --filter Category=Integration
```

**Important**: Integration tests create real resources. Clean up after testing to avoid charges.

## Project Structure

```
updown-dotnet/
├── src/
│   ├── Apis/              # API endpoint implementations
│   │   ├── ApiChecks.cs
│   │   ├── ApiDowntimes.cs
│   │   ├── ApiMetrics.cs
│   │   ├── ApiNodes.cs
│   │   ├── ApiPulse.cs
│   │   ├── ApiRecipients.cs
│   │   └── ApiStatusPages.cs
│   ├── Exceptions/        # Custom exception types
│   ├── Models/            # Request/response models
│   ├── UpdownClient.cs    # Main client class
│   ├── UpdownClientBase.cs # Base client functionality
│   ├── UpdownClientBuilder.cs # Fluent builder
│   └── UpdownClientFactory.cs # Factory methods
├── test/
│   └── UpdownDotNetTests/
│       ├── Checks/        # Check API tests
│       ├── Downtimes/     # Downtime API tests
│       ├── Metrics/       # Metrics API tests
│       ├── Nodes/         # Nodes API tests
│       ├── Pulse/         # Pulse API tests
│       ├── Recipients/    # Recipients API tests
│       └── StatusPages/   # Status pages API tests
└── docs/
    ├── ARCHITECTURE.md    # Architecture documentation
    ├── API_REFERENCE.md   # API reference
    └── CONTRIBUTING.md    # This file
```

## Coding Standards

### C# Conventions

- Follow standard C# naming conventions (PascalCase for types, camelCase for locals)
- Use meaningful variable and method names
- Keep methods focused and small (single responsibility)
- Use nullable reference types consistently
- Add XML documentation comments to all public APIs

### Code Style

```csharp
// Good
/// <summary>
/// Gets a specific check by its token.
/// </summary>
/// <param name="token">The check token.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>The check details.</returns>
/// <exception cref="ArgumentException">Thrown when token is null or empty.</exception>
public async Task<Check> CheckAsync(string token, CancellationToken cancellationToken = default)
{
    if (string.IsNullOrWhiteSpace(token))
        throw new ArgumentException("Token cannot be null or empty.", nameof(token));

    var uri = new Uri($"{ChecksPath}/{token}", UriKind.Relative);
    var result = await GetAsync<Check>(uri, cancellationToken).ConfigureAwait(false);
    return result;
}
```

### Nullable Reference Types

- Enable nullable reference types (`<Nullable>enable</Nullable>`)
- Mark all nullable parameters and properties with `?`
- Use null-forgiving operator `!` sparingly and only when certain
- Validate parameters and throw appropriate exceptions

### Async/Await

- All API methods must be async
- Use `ConfigureAwait(false)` for library code
- Accept `CancellationToken cancellationToken = default` parameter
- Name async methods with `Async` suffix

### Error Handling

- Throw specific exception types (ArgumentException, UpdownApiException, etc.)
- Include helpful error messages
- Capture and include response content in API exceptions
- Don't catch exceptions unless you can handle them meaningfully

## Testing Guidelines

### Unit Test Structure

Use the **Arrange-Act-Assert** pattern:

```csharp
[Test]
public async Task CheckAsync_ReturnsCheck_WhenSuccessful()
{
    // Arrange
    var mockResult = new Check { Token = "test-token", Url = "https://example.com" };
    
    Server.Given(Request.Create()
            .WithPath($"/{UpdownClient.ChecksPath}/test-token")
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithBodyAsJson(mockResult));

    var client = UpdownClientFactory.Create(Server.CreateClient());

    // Act
    var result = await client.CheckAsync("test-token");

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Token, Is.EqualTo("test-token"));
    Assert.That(result.Url, Is.EqualTo("https://example.com"));
}
```

### Test Naming

Use descriptive test names that indicate:
- What is being tested
- Under what conditions
- What is the expected outcome

Format: `MethodName_ExpectedBehavior_WhenCondition`

Examples:
- `CheckAsync_ReturnsCheck_WhenSuccessful`
- `CheckAsync_ThrowsArgumentException_WhenTokenIsNull`
- `CheckAsync_ThrowsNotFoundException_WhenCheckDoesNotExist`

### Test Coverage

Aim for:
- **90%+ code coverage** for critical paths
- **100% coverage** for public APIs
- **Error path testing** for all exception scenarios
- **Edge case testing** (null, empty, boundary values)

### Mocking

Use WireMock.NET for HTTP mocking:

```csharp
public class MyTests : BaseHttpClientTest
{
    [Test]
    public async Task Example()
    {
        // Mock HTTP response
        Server.Given(Request.Create()
                .WithPath("/api/endpoint")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new { data = "value" }));

        // Test code...
    }
}
```

## Pull Request Process

### Before Submitting

1. **Update from upstream**:
```bash
git fetch upstream
git rebase upstream/main
```

2. **Run tests**:
```bash
dotnet test
```

3. **Check for warnings**:
```bash
dotnet build /warnaserror
```

4. **Update documentation** if you've changed:
   - Public API
   - Behavior
   - Configuration

### Commit Messages

Write clear, concise commit messages:

```
Short summary (50 chars or less)

Longer explanation if needed. Wrap at 72 characters.
Explain the problem this commit solves and why you
chose this solution.

Fixes #123
```

**Commit Message Guidelines**:
- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- First line should be capitalized
- No period at the end of the summary line
- Reference issues and pull requests

### Pull Request Template

When creating a PR, include:

1. **Description**: What does this PR do?
2. **Motivation**: Why is this change needed?
3. **Testing**: How was this tested?
4. **Breaking Changes**: Any breaking changes?
5. **Checklist**:
   - [ ] Tests added/updated
   - [ ] Documentation updated
   - [ ] No breaking changes (or documented if present)
   - [ ] All tests passing
   - [ ] No compiler warnings

### Review Process

1. A maintainer will review your PR
2. Address any feedback
3. Once approved, a maintainer will merge

## Release Process

(For maintainers only)

### Versioning

Follow [Semantic Versioning](https://semver.org/):
- **MAJOR** version for breaking changes
- **MINOR** version for new features (backward compatible)
- **PATCH** version for bug fixes

### Release Checklist

1. Update version in `src/UpdownDotnet.csproj`
2. Update `PackageReleaseNotes`
3. Update README.md if needed
4. Create git tag: `git tag v1.x.x`
5. Push tag: `git push origin v1.x.x`
6. Build NuGet package: `dotnet pack -c Release`
7. Push to NuGet.org
8. Create GitHub release

## Development Tips

### Debugging

Add this to launchSettings.json for easier debugging:
```json
{
  "profiles": {
    "UpdownDotnet.Tests": {
      "commandName": "Project",
      "environmentVariables": {
        "UPDOWN_API_KEY": "your-test-key"
      }
    }
  }
}
```

### IDE Configuration

**Visual Studio**:
- Enable "Remove unused usings" on save
- Enable "Format document" on save
- Install ReSharper (optional but recommended)

**VS Code**:
- Install C# extension
- Install C# XML Documentation Comments extension
- Configure format on save

### Common Tasks

**Add new API endpoint**:
1. Create model classes in `src/Models/`
2. Add API methods to appropriate file in `src/Apis/`
3. Add XML documentation comments
4. Create tests in `test/UpdownDotNetTests/`
5. Update README.md with examples
6. Update API_REFERENCE.md

**Fix a bug**:
1. Write a failing test that reproduces the bug
2. Fix the bug
3. Verify the test passes
4. Check for similar bugs elsewhere
5. Submit PR with test and fix

## Questions?

If you have questions:
1. Check existing documentation
2. Search closed issues
3. Open a new issue with the "question" label

## License

By contributing, you agree that your contributions will be licensed under the project's MIT License.

## Thank You!

Your contributions help make this project better for everyone. Thank you for taking the time to contribute!

