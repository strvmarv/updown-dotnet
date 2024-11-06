# updown-dotnet
A simple Updown.io .NET Client

# Usage

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

## Run Tests
```bash
dotnet test
```

# Notes

- This client is a simple wrapper around the Updown.io API. It does not implement all the API endpoints.
- The client uses the System.Text.Json namespace to serialize and deserialize JSON data.
- The client is asynchronous and uses the HttpClient class to make HTTP requests to the Updown.io API.
- The HttpClient is implemented per Micrsoft recommendations.  In this case, a Singleton that is reused.
- You may provide your own HttpClient instance if you want to manage the lifecycle of the HttpClient.
- Manual tests are provided if you'd like to observe the client in action.  You will need to provide your own API key.