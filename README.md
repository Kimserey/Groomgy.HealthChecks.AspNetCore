# Groomgy.HealthChecks.AspNetCore

A Health Check library for ASP.NET Core.

| Package | Appveyor build | NuGet package |
|---------|----------------|---------------|
| Groomgy.HealthChecks.AspNetCore         | [![Build status](https://ci.appveyor.com/api/projects/status/eouclq5a93ix6yp8?svg=true)](https://ci.appveyor.com/project/Kimserey16189/groomgy-healthchecks-aspnetcore) | [![NuGet](https://img.shields.io/nuget/v/Groomgy.HealthChecks.AspNetCore.svg?style=flat&colorB=blue)](http://www.nuget.org/packages/Groomgy.HealthChecks.AspNetCore) |
| Groomgy.HealthChecks.AspNetCore.Sqlite  | [![Build status](https://ci.appveyor.com/api/projects/status/eouclq5a93ix6yp8?svg=true)](https://ci.appveyor.com/project/Kimserey16189/groomgy-healthchecks-aspnetcore) | [![NuGet](https://img.shields.io/nuget/v/Groomgy.HealthChecks.AspNetCore.Sqlite.svg?style=flat&colorB=blue)](http://www.nuget.org/packages/Groomgy.HealthChecks.AspNetCore.Sqlite) |
| Groomgy.HealthChecks.AspNetCore.Orleans | [![Build status](https://ci.appveyor.com/api/projects/status/eouclq5a93ix6yp8?svg=true)](https://ci.appveyor.com/project/Kimserey16189/groomgy-healthchecks-aspnetcore) | [![NuGet](https://img.shields.io/nuget/v/Groomgy.HealthChecks.AspNetCore.Orleans.svg?style=flat&colorB=blue)](http://www.nuget.org/packages/Groomgy.HealthChecks.AspNetCore.Orleans) |

## Install

```
Install-Package Groomgy.HealthChecks.AspNetCore
```

## Use

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

```      
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseHealthChecksEndpoint();
}
```