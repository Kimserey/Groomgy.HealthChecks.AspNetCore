# Groomgy.HealthChecks.AspNetCore

A Health Check library for ASP.NET Core.

| Package | Appveyor build | NuGet package |
|---------|----------------|---------------|
| Groomgy.HealthChecks.AspNetCore         | [![Build status](https://ci.appveyor.com/api/projects/status/eouclq5a93ix6yp8?svg=true)](https://ci.appveyor.com/project/Kimserey16189/groomgy-healthchecks-aspnetcore) | [![NuGet](https://img.shields.io/nuget/v/Groomgy.HealthChecks.AspNetCore.svg?style=flat&colorB=blue)](http://www.nuget.org/packages/Groomgy.HealthChecks.AspNetCore) |
| Groomgy.HealthChecks.AspNetCore.Sqlite  | [![Build status](https://ci.appveyor.com/api/projects/status/eouclq5a93ix6yp8?svg=true)](https://ci.appveyor.com/project/Kimserey16189/groomgy-healthchecks-aspnetcore) | [![NuGet](https://img.shields.io/nuget/v/Groomgy.HealthChecks.AspNetCore.Sqlite.svg?style=flat&colorB=blue)](http://www.nuget.org/packages/Groomgy.HealthChecks.AspNetCore.Sqlite) |
| Groomgy.HealthChecks.AspNetCore.Orleans | [![Build status](https://ci.appveyor.com/api/projects/status/eouclq5a93ix6yp8?svg=true)](https://ci.appveyor.com/project/Kimserey16189/groomgy-healthchecks-aspnetcore) | [![NuGet](https://img.shields.io/nuget/v/Groomgy.HealthChecks.AspNetCore.Orleans.svg?style=flat&colorB=blue)](http://www.nuget.org/packages/Groomgy.HealthChecks.AspNetCore.Orleans) |


## Install library

Install the healthchecks library with the following command:

```
Install-Package Groomgy.HealthChecks.AspNetCore
```

## Use the healthchecks

The library contains an extension on the `IServiceCollection` which provides an `Action<IHealthCheckBuilder>`. 
The builder allows to register different healthchecks.

For example here we are registering a self check which would directly reply OK 200 with a message `MyApi is running` and a Url check which would `GET` the endpoint provided.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddHealthChecks(c =>
    {
        c.AddSelfCheck("MyApi is running.");
        c.AddUrlCheck("Authority is accessible.", "https://my-identity/.well-known/openid-configuration");
    });
}
```

It is also possible to add custom healthchecks with `c.Add(...)` which takes a function providing the service provider and expect an implementation of `IHealthCheck`.
Then we use the healthchecks by registering the middleware on the `IApplicationBuilder`.

```      
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseHealthChecksEndpoint();
}
```

## Use in your own classes

The service collection extension also register a `IHealthCheckService` which can be used by dependency injection.

```
public class MyClass
{
	IHealthCheckService _service;

	public MyClass(IHealthCheckService service)
	{
		_service = serivce;
	}

	public async Task Check()
	{
		var results = await _service.CheckAll();

		...
	}
}
```

The result will be a `CompositeHealthCheckResult` which combine all checks together and return a failure if any of the check fails.

```
{
  "results": [
    {
      "name": "SelfCheck",
      "status": "Healthy",
      "message": "WebA is running."
    },
    {
      "name": "UrlCheck",
      "status": "Healthy",
      "message": "Successfully contacted url 'https://my-identity/.well-known/openid-configuration'. Message: Authority is accessible."
    }
  ],
  "compositeStatus": "Healthy"
}
```

If for example, the remote dependency is not accessible, the check will be unhealthy.

```
{
  "results": [
    {
      "name": "SelfCheck",
      "status": "Healthy",
      "message": "WebA is running."
    },
    {
      "name": "UrlCheck",
      "status": "Unhealthy",
      "message": "Failed to contact url 'https://my-identity/.well-known/openid-configuration'. Message: The delegate executed asynchronously through TimeoutPolicy did not complete within the timeout."
    }
  ],
  "compositeStatus": "Unhealthy"
}
```

## Url check policies

The default `UrlCheck` implements two Polly policies, `WaitAndRetry` and `Timeout`. 

```
public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry(int retryCount = 5) =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .Or<TimeoutRejectedException>()
        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

public static IAsyncPolicy<HttpResponseMessage> Timeout(int seconds = 2) =>
    Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
```

An extension on the service collection can be used to override those policies and set your own. For example if we do not want the `WaitAndRetry` but only want the `Timeout`, we could do it like so:

```
services.AddHealthChecks(c => ..., new[] { PolicyHandler.Timeout(2) });
```

## Health endpoint path

The default endpint is on `/health` but can be overriden with the configuration in `appsettings.json`:

```
{
  "endpoints": {
    "health": "/internal/__health"
  }
}
```

## Sqlite Healthcheck extension

The Sqlite healtcheck can be used to check the exitence of database and tables.
Install the extension with the following command:

```
Install-Package Groomgy.HealthChecks.AspNetCore.Sqlite
```

And register the healthcheck:

```
c.AddSqliteCheck("[myconnectionstring]", "tableA", "tableB");
```

The existence of `tableA` and `tableB` will be checked against the database from the connection string provided.

## Microsoft.Orleans Healthcheck extension

For Microsoft.Orleans, the healtcheck is build to ensure that the client can access the Orleans cluster. It is done via a single grain implementing the `IAmAlive` grain interface.
Install the extension with the following command:

```
Install-Package Groomgy.HealthChecks.AspNetCore.Orleans
```

The extension needs to be installed in the projects:

 1. Client project
 2. Grain project

From the client project, register the 

```
var client = builder
    .ConfigureApplicationParts(partManager =>
    {
		// ... other application part
        partManager.AddApplicationPart(typeof(IIAmAlive).Assembly).WithReferences();
    })
    .Build();
```

Next create the grain in your grain project:

```
[StatelessWorker]
public class IAmAliveGrain : Grain, IIAmAlive
{
    public Task<bool> Check()
    {
        return Task.FromResult(true);
    }
}
```

Lastly register the healthcheck:

```
c.AddOrleansClientCheck();
```

## License

MIT
