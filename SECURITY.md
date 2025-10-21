# Security Policy

## Supported Versions

We actively support the following versions with security updates:

| Version | Supported          | End of Support |
| ------- | ------------------ | -------------- |
| 2.0.x   | :white_check_mark: | Active         |
| 1.1.x   | :white_check_mark: | 2026-01-31     |
| 1.0.x   | :x:                | 2025-10-21     |
| < 1.0   | :x:                | End of Life    |

## Reporting a Vulnerability

If you discover a security vulnerability in this project, please help us address it responsibly:

1. **DO NOT** open a public GitHub issue for security vulnerabilities
2. **DO** create a [security advisory](https://github.com/strvmarv/updown-dotnet/security/advisories/new) using GitHub's private reporting feature
3. **DO** include:
   - Description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Suggested fix (if available)

We will:
- Acknowledge receipt within 48 hours
- Provide an estimated timeline for a fix
- Notify you when the vulnerability is fixed
- Credit you in the security advisory (unless you prefer to remain anonymous)

## API Key Security

### Best Practices

When using this library:

✅ **DO:**
- Store API keys in environment variables or secure configuration
- Use separate API keys for development, testing, and production
- Rotate API keys regularly
- Use .NET Secret Manager for local development
- Add `*.env`, `*secrets.json` to `.gitignore`
- Use Azure Key Vault, AWS Secrets Manager, or similar for production

❌ **DON'T:**
- Hardcode API keys in source code
- Commit API keys to version control
- Share API keys in issues or pull requests
- Log API keys in application logs
- Include API keys in client-side code

### For Contributors

If you're contributing to this project:

1. **Never commit real API keys** - All test files use placeholders like `"YOUR-API-KEY-HERE"`
2. **Check before committing:**
   ```bash
   git diff --cached | grep -i "api.key"
   git diff --cached | grep -E "[a-z0-9]{32,}"
   ```
3. **Use environment variables** for manual testing - See `.env.example`
4. **Configuration files** with secrets are git-ignored - See `.gitignore`

### For Users

To keep your API keys secure:

```csharp
// ✅ Good: Environment variable
var apiKey = Environment.GetEnvironmentVariable("UPDOWN_API_KEY");
var client = new UpdownClientBuilder()
    .WithApiKey(apiKey)
    .Build();

// ✅ Good: Configuration (with User Secrets in development)
var apiKey = configuration["Updown:ApiKey"];
var client = new UpdownClientBuilder()
    .WithApiKey(apiKey)
    .Build();

// ❌ Bad: Hardcoded
var client = new UpdownClientBuilder()
    .WithApiKey("ro-abc123xyz...") // NEVER DO THIS
    .Build();
```

### Setting Up User Secrets (Recommended for Development)

```bash
# Navigate to your project directory
cd YourProject/

# Initialize user secrets
dotnet user-secrets init

# Set your API key
dotnet user-secrets set "Updown:ApiKey" "your-api-key-here"
```

In your code:
```csharp
// IConfiguration is injected via DI
var apiKey = configuration["Updown:ApiKey"];
```

## Dependencies

We regularly update dependencies to address security vulnerabilities. To check for updates:

```bash
dotnet list package --outdated
dotnet list package --vulnerable
```

## Security Features in This Library

- **No credential storage** - API keys are only held in memory during requests
- **HTTPS only** - All API calls use TLS 1.2+
- **Input validation** - Parameters are validated before sending
- **No logging of sensitive data** - API keys are never logged
- **Thread-safe** - `HttpClient` management follows Microsoft best practices

## Additional Resources

- [OWASP API Security Top 10](https://owasp.org/www-project-api-security/)
- [.NET Security Best Practices](https://docs.microsoft.com/en-us/dotnet/standard/security/)
- [Managing Secrets in .NET](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Updown.io Security](https://updown.io/security)

---

Last Updated: 2025-10-21
