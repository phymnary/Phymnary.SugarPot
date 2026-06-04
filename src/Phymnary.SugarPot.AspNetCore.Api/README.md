# Phymnary.SugarPot.AspNetCore.Api

ASP.NET Core API utilities for SugarPot, including endpoint abstractions, runtime web helpers, and Roslyn-based endpoint source generation.

This project works together with `Phymnary.SugarPot.AspNetCore.Api.Roslyn` to reduce boilerplate for minimal APIs by generating endpoint mapping code from attributes.

## Features

- Endpoint abstraction via `IEndpoint`
- Attribute-driven endpoint generation with `[Endpoint]`
- Group route pattern support with `[RoutePattern]`
- Group/endpoint route builder customization with `[RouteBuilder]`
- API schema grouping with `[ApiSchema]`
- Extensions to map endpoints and bootstrap runtime services:
  - `MapEndpoint<TEndpoint>()`
  - `AddBoilerplateServices()`
  - `UseBoilerplateServices()`
- Built-in HTTP-context based providers:
  - `ICurrentUser`
  - `ICurrentTenant`
  - `IAbortedToken`
- Exception handling integration (`AspExceptionHandler`, ProblemDetails)

---

## Source Generator: Api.Roslyn

The companion generator project (`Phymnary.SugarPot.AspNetCore.Api.Roslyn`) provides:

- **Incremental source generator** for endpoint mapping code
- **Analyzer diagnostics** to validate endpoint contracts

### What it generates

For classes marked with `[Endpoint]`, the generator emits code that:

- implements endpoint mapping based on HTTP method and route pattern
- uses `HandleAsync` as the handler method
- applies route customization from endpoint-level `BuildRoute(...)` or nearest group `[RouteBuilder]` method

For classes marked with `[ApiSchema]`, it emits schema mapping entry points that map discovered endpoints.

### Current diagnostic

- `SPAPI001` — Missing `HandleAsync` method for `[Endpoint]` classes.

---

## Installation

Add a project/package reference to `Phymnary.SugarPot.AspNetCore.Api`.

When consumed as a package in `Release`, the Roslyn analyzer/generator assembly is packed under `analyzers/dotnet/cs` automatically by this project setup.

---

## Quick Start

### 1) Register API runtime services

```csharp
using Phymnary.SugarPot.AspNetCore.Extensions;

builder.Services.AddBoilerplateServices();
```

### 2) Add middleware integration

```csharp
using Phymnary.SugarPot.AspNetCore.Extensions;

var app = builder.Build();
app.UseBoilerplateServices();
```

### 3) Create an endpoint with source generation

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

You can define namespace-near static route helpers and apply them to endpoint groups.

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

Endpoint-local members (`RoutePattern` property, `BuildRoute` method) override group defaults.

---

## Manual Endpoint Mapping (without generator)

If needed, you can map endpoints directly:

```csharp
using Phymnary.SugarPot.AspNetCore.Api.Extensions;

app.MapEndpoint<GetHealth>();
```

---

## Runtime Claims Mapping

`UseBoilerplateServices()` reads claims and maps them to scoped providers:

- `SubClaimName` (default: `"sub"`) -> `ICurrentUser.Id`
- `TenantClaimName` (default: `"tenant"`) -> `ICurrentTenant.Id`

You can change claim names via:

- `WebApplicationBuilderExtensions.SubClaimName`
- `WebApplicationBuilderExtensions.TenantClaimName`

---

## Notes

- `[Endpoint]` classes must provide a `HandleAsync` method.
- HTTP method is resolved from:
  1. `[Endpoint(Method.X)]` argument,
  2. class name prefix (`Get*`, `Post*`, etc.),
  3. namespace suffix (`.get`, `.post`, etc.).
- Keep endpoint classes `partial` to align with generated partial output.

## Target Frameworks

- .NET Standard 2.1
