# Phymnary.SugarPot.DependencyInjection

Lightweight primitives for convention-based dependency registration.

This project provides simple contracts and attributes to standardize how services are discovered and added to the .NET `IServiceCollection`.

## Features

- `IAutoRegister` interface for explicit dependency registration
- `[Auto]` marker attribute for discoverable classes
- `[Service(Lifetime)]` attribute to declare service lifetime
- `Lifetime` enum (`Singleton`, `Scoped`, `Transient`)
- Optional self-registration with `ServiceAttribute.IsSelf`

## Installation

`Install-Package Phymnary.SugarPot.DependencyInjection`

## API Overview

### `IAutoRegister`

Implement this interface when a module/class should register dependencies manually:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.DependencyInjection;

public sealed class UserAutoRegister : IAutoRegister
{
    public void AddDependencies(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
```

### `[Auto]`

Use this marker attribute to indicate a class is eligible for automatic discovery/registration by consumers:

```csharp
using Phymnary.SugarPot.DependencyInjection;

[Auto]
public sealed class BackgroundJobRunner
{
}
```

### `[Service]`

Use this attribute to declare the intended lifetime:

```csharp
using Phymnary.SugarPot.DependencyInjection;

[Service(Lifetime.Scoped)]
public sealed class UserService : IUserService
{
}
```

Register as self (concrete type) when needed:

```csharp
[Service(Lifetime.Singleton, IsSelf = true)]
public sealed class CacheWarmupService
{
}
```

## Notes

- This project intentionally stays minimal and framework-agnostic.
- Actual assembly scanning/registration behavior is implemented by consuming projects.

## Target Frameworks

- .NET Standard 2.1
