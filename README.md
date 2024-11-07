[![build and test](https://github.com/strvmarv/updown-dotnet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/strvmarv/updown-dotnet/actions/workflows/build-and-test.yml)

# updown-dotnet
A simple Updown.io .NET Client

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

# Usage

Example usage using Checks.  Implementation across entities is similar.  Though some entities may not support all methods.  

[!IMPORTANT]
Use manual tests for reference.

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
```