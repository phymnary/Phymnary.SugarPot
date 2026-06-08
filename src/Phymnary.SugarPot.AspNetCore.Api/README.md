# Phymnary.SugarPot.AspNetCore.Api

ASP.NET Core API utilities for SugarPot, including endpoint abstractions, runtime web helpers, and a Roslyn-based endpoint source generator.

This project works together with Phymnary.SugarPot.AspNetCore.Api.Roslyn to reduce boilerplate for minimal APIs by generating endpoint mapping code from attributes and analyzers.

## Table of contents

- Features
- Source generator (Api.Roslyn)
- Installation
- Getting started
- Group route configuration
- Manual endpoint mapping
- Runtime claims mapping
- Notes & troubleshooting
- Target frameworks
- Contributing
- License

## Features

- Endpoint abstraction via `IEndpoint`
- Attribute-driven endpoint generation with `[Endpoint]`
- Group route pattern support with `[RoutePattern]`
- Group/endpoint route builder customization with `[RouteBuilder]`
- API schema grouping with `[ApiSchema]`
- Runtime mapping helpers: `MapEndpoint<TEndpoint>()`, `AddBoilerplateServices()`, `UseBoilerplateServices()`
- Built-in HTTP-context based providers: `ICurrentUser`, `ICurrentTenant`, `IAbortedToken`
- Exception handling integration (`AspExceptionHandler`, ProblemDetails)

---

## Source Generator: Api.Roslyn

The companion generator project (`Phymnary.SugarPot.AspNetCore.Api.Roslyn`) provides:

- Incremental source generator for endpoint mapping code
- Analyzer diagnostics to validate endpoint contracts

### What it generates

For classes marked with `[Endpoint]`, the generator emits code that:

- implements endpoint mapping based on HTTP method and route pattern
- uses `HandleAsync` as the handler method
- applies route customization from endpoint-level `BuildRoute(...)` or nearest group `[RouteBuilder]` method

For classes marked with `[ApiSchema]`, it emits schema mapping entry points that map discovered endpoints.

### Current diagnostic

- `SPAPI001` — Missing `HandleAsync` method for `[Endpoint]` classes.

### Packaging note

When this package is produced as a NuGet package the Roslyn analyzer/generator assembly is placed under `analyzers/dotnet/cs` so consumers receive generator functionality automatically when the package is referenced.

---

## Installation

`dotnet add package Phymnary.SugarPot.AspNetCore.Api`

## Getting started

### 1) Register runtime services

```csharp
using Phymnary.SugarPot.AspNetCore.Extensions;

builder.Services.AddApiServices().AddBoilerplateExceptionHandler();
```

### 2) Add middleware

```csharp
var app = builder.Build();
app.UseBoilerplateServices();
```

### 3) Create an endpoint (generator will produce mapping)

```csharp
using Microsoft.AspNetCore.Builder;
using Phymnary.SugarPot.AspNetCore.Api;

[Endpoint(Method.Get)]
public partial class GetHealth
{
    private static string RoutePattern => "/health";

    private static IResult HandleAsync()
    {
        return Results.Ok(new { ok = true });
    }

    private static RouteHandlerBuilder BuildRoute(RouteHandlerBuilder builder)
    {
        return builder.WithTags("Health");
    }
}
```

---

## Group Route Configuration

You can define namespace-level static route helpers and apply them to endpoint groups.

### Shared route pattern

```csharp
using Phymnary.SugarPot.AspNetCore.Api;

public static class UserRouteConfig
{
    [RoutePattern]
    public static string GetRoutePattern<TEndpoint>(TEndpoint endpoint)
        where TEndpoint : class, IEndpoint
    {
        return "/api/users";
    }
}
```

### Shared route builder

```csharp
using Microsoft.AspNetCore.Builder;
using Phymnary.SugarPot.AspNetCore.Api;

public static class UserRouteBuilderConfig
{
    [RouteBuilder]
    public static RouteHandlerBuilder Build(RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization();
    }
}
```

Endpoint-local `RoutePattern` property and `BuildRoute` method override group defaults.

---

## Manual Endpoint Mapping

If you prefer not to use the generator you can map endpoints manually:

```csharp
using Phymnary.SugarPot.AspNetCore.Api.Extensions;

app.MapEndpoint<GetHealth>();
```

---

## Runtime Claims Mapping

`UseBoilerplateServices()` maps claims to scoped providers:

- `SubClaimName` (default: `"sub"`) -> `ICurrentUser.Id`
- `TenantClaimName` (default: `"tenant"`) -> `ICurrentTenant.Id`

Claim names can be customized via `WebApplicationBuilderExtensions.SubClaimName` and `WebApplicationBuilderExtensions.TenantClaimName`.

---

## Notes & troubleshooting

- `[Endpoint]` classes must provide a `HandleAsync` method (see `SPAPI001` diagnostic).
- HTTP method resolution order: attribute (`[Endpoint(Method.X)]`), class name prefix (`Get*`, `Post*`, etc.), namespace suffix (`.get`, `.post`, etc.).
- Keep endpoint classes `partial` to align with generated partial output.

### Troubleshooting:

- If generator diagnostics do not appear, ensure the consuming project references the package that contains the analyzer (or adds the analyzer project as an analyzer during development).
- If mapping is not generated, confirm the endpoint class is public/partial and follows the required shape for `HandleAsync`.

## Target frameworks

This library is built to support multiple TFMs commonly used in the solution. Typical TFMs:

- net8.0
- net9.0
- net10.0

Check the project file for exact TFMs.

---

## Contributing

Contributions, bug reports and feature requests are welcome. Please open issues or pull requests on the repository.

## License

See the repository root for license information.
