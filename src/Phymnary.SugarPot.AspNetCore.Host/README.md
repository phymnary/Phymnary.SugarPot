# Phymnary.SugarPot.AspNetCore.Host

Host-level bootstrapping helpers for SugarPot ASP.NET Core applications.

This project provides host/runtime extensions used by application entry points, with a focus on standardized configuration loading and a small set of helpers to streamline app startup.

## Features

- ConfigurationExtensions.AddDefaults(...) for conventional configuration sources
- Integration helpers for WebApplication and Generic Host setup
- Designed to work with SugarPot API and EF Core packages

## Installation

`dotnet add package Phymnary.SugarPot.AspNetCore.Host`

## Configuration Defaults

AddDefaults(this IConfigurationBuilder builder, string env) adds configuration sources in this order:

1. appsettings.json (required)
2. appsettings.{env}.json (optional)
3. appsettings.{env}.user.json (optional)
4. environment variables

This ordering allows environment-specific and per-developer overrides while preserving environment variables as the last override.

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

This host project is intended to be used together with:

- Phymnary.SugarPot.AspNetCore.Api
- Phymnary.SugarPot.AspNetCore.EntityFrameworkCore

## Target frameworks

This package targets multiple TFMs used in the solution. Typical targets include:

- net8.0
- net9.0
- net10.0

Check the project file for exact TFMs.

---

## Contributing

Contributions, bug reports and feature requests are welcome. Please open issues or pull requests on the repository.

## License

See the repository root for license information.
