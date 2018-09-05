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

## License

MIT