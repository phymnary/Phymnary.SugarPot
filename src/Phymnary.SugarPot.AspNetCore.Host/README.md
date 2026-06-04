# Phymnary.SugarPot.AspNetCore.Host

Host-level bootstrapping helpers for SugarPot ASP.NET Core applications.

This project provides host/runtime extensions used by application entry points, with a focus on standardized configuration loading.

## Features

- `ConfigurationExtensions.AddDefaults(...)` for conventional configuration sources
- Integrates cleanly with ASP.NET Core host setup
- Designed to work with SugarPot API and EF Core packages

## Installation

Add a project/package reference to `Phymnary.SugarPot.AspNetCore.Host`.

## Configuration Defaults

`AddDefaults(this IConfigurationBuilder builder, string env)` adds configuration sources in this order:

1. `appsettings.json` (required)
2. `appsettings.{env}.json` (optional)
3. `appsettings.{env}.user.json` (optional)
4. environment variables

## Usage Example

```csharp
using Phymnary.SugarPot.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddDefaults(builder.Environment.EnvironmentName);
```

## Why use this

- Keeps configuration bootstrapping consistent across services
- Supports environment-specific and user-local overrides
- Preserves standard ASP.NET Core environment variable behavior

## Related Packages

This host project references:

- `Phymnary.SugarPot.AspNetCore.Api`
- `Phymnary.SugarPot.AspNetCore.EntityFrameworkCore`

## Target Frameworks

- .NET Standard 2.0
- .NET 8
