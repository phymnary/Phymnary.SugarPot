# Phymnary.SugarPot.DependencyInjection.Roslyn

Roslyn analyzer + incremental source generator for convention-based dependency registration.

This project works with `Phymnary.SugarPot.DependencyInjection` attributes to generate `IServiceCollection` registration code at compile time.

## What this project provides

- Incremental generator: `AutoRegisterGenerator`
- Analyzer: `AutoRegisterAnalyzer`
- Diagnostic descriptors for invalid service decorations

## Generation model

The generator scans for:

- `[Service(Lifetime.X)]` on implementation classes
- `[Auto]` on module classes (must be `partial`)

For each `[Auto]` partial class, it generates registration code that adds all discovered `[Service]` types to `IServiceCollection` using the configured lifetime.

## Service type resolution rules

For a class decorated with `[Service]`, generated registration chooses service type in this order:

1. If `IsSelf = true` -> register implementation as itself
2. Otherwise first implemented interface of the class
3. Otherwise first interface from base type
4. Otherwise implementation type itself

Lifetime is mapped to:

- `Lifetime.Singleton` -> `AddSingleton<TService, TImpl>`
- `Lifetime.Scoped` -> `AddScoped<TService, TImpl>`
- `Lifetime.Transient` -> `AddTransient<TService, TImpl>`

## Analyzer diagnostics

Current diagnostic:

- `SPDI0001` (Error): class with `[Service]` cannot be abstract or generic.

## Conventions and requirements

- `[Auto]` target class must be `partial` (required for source generation).
- `[Service]` classes must be concrete, non-generic classes.
- Generator relies on attributes from `Phymnary.SugarPot.DependencyInjection` namespace.

## Example

```csharp
using Phymnary.SugarPot.DependencyInjection;

[Service(Lifetime.Scoped)]
public sealed class UserService : IUserService
{
}

[Auto]
public partial class ApplicationModule
{
}
```

Generated code will add a scoped registration similar to:

```csharp
services.AddScoped<IUserService, UserService>();
```

## Project structure

- `Components/AutoRegister/AutoRegisterGenerator*.cs` - generator pipeline + syntax emit
- `Components/AutoRegister/AutoRegisterAnalyzer.cs` - semantic validation
- `Diagnostics/DiagnosticDescriptors.cs` - analyzer metadata
- `Constants/GeneratorConstant.cs` - shared constants

## Dependencies

- `Microsoft.CodeAnalysis.Analyzers`
- `Microsoft.CodeAnalysis.CSharp.Workspaces`

## Local development notes

- Build this project to validate generator/analyzer behavior.
- Validate generated output in consumer projects using `[Auto]` and `[Service]`.
- Keep analyzer metadata in sync with `AnalyzerReleases.Shipped.md` and `AnalyzerReleases.Unshipped.md`.

## Target framework

- .NET Standard 2.0 (analyzer/generator compatibility target)
