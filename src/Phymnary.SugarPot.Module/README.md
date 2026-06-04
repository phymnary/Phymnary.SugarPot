# Phymnary.SugarPot.Module

Lightweight module helpers for .NET applications.

This project provides a small module pattern to organize service registration and configuration binding in a consistent way.

## Features

- `IModule` contract for module-based service registration
- `AddModule<TModule>()` extension for `IHostApplicationBuilder`
- Optional integration with `IAutoRegister` from `Phymnary.SugarPot.DependencyInjection`
- `IAppSettingsSection` pattern for strongly-typed configuration sections
- `ConfigureSection<TConfig>()` helpers for binding and validating options
- Utility extension methods for strings, enums, collections, reflection attributes, and enumerables

## Installation

Install `Phymnary.SugarPot.Module` from NuGet Package Manager.

## Quick Start

### 1) Create a module

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.Module;

public sealed class UserModule : IModule
{
    public void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddOptions();
    }
}
```

### 2) Register the module

```csharp
using Phymnary.SugarPot.Module.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.AddModule<UserModule>();
```

## Configuration Section Binding

### Define config model

```csharp
using Phymnary.SugarPot.Module;

public sealed class JwtOptions : IAppSettingsSection
{
    public static string Section => "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
```

### Bind section

```csharp
using Phymnary.SugarPot.Module.Extensions;

services.ConfigureSection<JwtOptions>();
```

### Bind section with validator

```csharp
services.ConfigureSection<JwtOptions, JwtOptionsValidator>();
```

## Extension Method Highlights

- `PascalToKebabCase()`
- `SplitBySemicolon()`
- `IsBlank()`, `TryGetValuable()`, `IsTruthy()`
- `TryGetExplicit()` for enums
- `AddIfNotExisted()`
- `AppendIfNotNull()`, `WhereNotNull()`, `JoinAsString()`
- `HasAttribute<TAttribute>()`

## Target Frameworks

- .NET Standard 2.1
