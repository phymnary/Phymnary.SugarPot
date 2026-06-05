# Phymnary.SugarPot.RoslynComponents

Shared Roslyn utilities for analyzers and source generators in the SugarPot solution.

This project provides reusable extensions, models, and high-performance helper types used by Roslyn-based projects such as `Phymnary.SugarPot.AspNetCore.Api.Roslyn` and `Phymnary.SugarPot.DependencyInjection.Roslyn`.

## Features

- Symbol and attribute inspection helpers
- Incremental provider utilities (including grouping helpers)
- Shared source diagnostic model and reporting extensions
- Type hierarchy modeling for generated partial type output
- Performance-oriented helper types:
  - `ImmutableArrayBuilder<T>`
  - `EquatableArray<T>`
  - pooled/object-pool and hash helpers

## Core namespaces

- `Phymnary.SugarPot.RoslynComponents.Extensions`
- `Phymnary.SugarPot.RoslynComponents.Models`
- `Phymnary.SugarPot.RoslynComponents.Helpers`

## Notable APIs

### Symbol extensions

`SymbolExtensions` includes helpers such as:

- `GetFullyQualifiedName(...)`
- `GetFullyQualifiedMetadataName(...)`
- `HasAttributeWithFullyQualifiedMetadataName(...)`
- `ImplementsFromFullyQualifiedMetadataName(...)`
- `InheritsFromFullyQualifiedMetadataName(...)`

These are useful for robust semantic checks in analyzers/generators.

### Attribute helpers

`AttributeDataExtensions.GetNamedArgument(...)` simplifies extraction of named attribute arguments.

### Incremental provider helpers

`ProviderExtensions.GroupBy(...)` helps shape incremental pipelines by grouping paired inputs into equatable arrays.

### Hierarchy model and syntax generation

`HierarchyInfo` captures type metadata (name, namespace, nesting) and can emit a generated `CompilationUnitSyntax` with proper partial nesting and generated-file trivia.

### Diagnostics model

`SourceDiagnosticInfo` wraps descriptor + location + args and provides `ToDiagnostic()` to report diagnostics consistently.

### Performance helpers

`ImmutableArrayBuilder<T>` and `EquatableArray<T>` are designed to reduce allocations and improve equality behavior in incremental pipelines.

## Example usage

```csharp
using Microsoft.CodeAnalysis;
using Phymnary.SugarPot.RoslynComponents.Extensions;

static bool IsEndpoint(INamedTypeSymbol symbol)
{
    return symbol.HasAttributeWithFullyQualifiedMetadataName(
        "Phymnary.SugarPot.AspNetCore.Api.EndpointAttribute");
}
```

```csharp
using Phymnary.SugarPot.RoslynComponents.Models;

var diagnostic = SourceDiagnosticInfo.Create(descriptor, symbol, symbol.Name).ToDiagnostic();
context.ReportDiagnostic(diagnostic);
```

## Dependencies

- `Microsoft.CodeAnalysis.Analyzers`
- `Microsoft.CodeAnalysis.CSharp`

## Target framework

- .NET Standard 2.0

## Notes

- This is an internal shared library for Roslyn tooling projects.
- Keep abstractions generic and dependency-light so they can be reused across analyzers/generators.
