# Phymnary.SugarPot.AspNetCore.Api.Roslyn

Roslyn analyzer + incremental source generator for `Phymnary.SugarPot.AspNetCore.Api`.

This project generates endpoint mapping code from API attributes and validates endpoint contracts at compile time.

## What this project provides

- Incremental generator: `EndpointGenerator`
- Analyzer: `EndpointAnalyzer`
- Diagnostics descriptors for endpoint contract validation

## Generation model

The generator scans for attributes from `Phymnary.SugarPot.AspNetCore.Api`:

- `[Endpoint]` on endpoint classes
- `[ApiSchema]` on schema/controller classes
- `[RoutePattern]` on static group route-pattern methods
- `[RouteBuilder]` on static group route-builder methods

Then it emits `.g.cs` files to:

1. Build route mapping for each endpoint
2. Apply endpoint-level or nearest-group route customizations
3. Generate schema/controller-level endpoint mapping methods

## Endpoint conventions used by generator

For each `[Endpoint]` type:

- Handler method name is expected to be `HandleAsync`
- HTTP method is resolved in this order:
  1. explicit `[Endpoint(Method.X)]`
  2. endpoint class name prefix (`Get*`, `Post*`, etc.)
  3. namespace suffix (`.get`, `.post`, etc.)
- Route pattern is resolved by:
  - endpoint member property `RoutePattern`, or
  - closest `[RoutePattern]` static group method
- Route builder customization is resolved by:
  - endpoint member method `BuildRoute(RouteHandlerBuilder)`, or
  - closest `[RouteBuilder]` static group method

## Analyzer diagnostics

Current diagnostic:

- `SPAPI001` (Error): class with `[Endpoint]` is missing `HandleAsync`.

## Project structure

- `Components/Endpoint/EndpointGenerator*.cs` - generator pipeline and syntax construction
- `Components/Endpoint/EndpointAnalyzer.cs` - semantic validation
- `Diagnostics/DiagnosticDescriptors.cs` - analyzer metadata
- `Constants/GeneratorConstant.cs` - shared generator constants

## Dependencies

- `Microsoft.CodeAnalysis.Analyzers`
- `Microsoft.CodeAnalysis.CSharp.Workspaces`
- `Phymnary.SugarPot.RoslynComponents`

## How it is consumed

This analyzer/generator is intended to be packaged with the runtime API package (`Phymnary.SugarPot.AspNetCore.Api`) and loaded by the compiler as an analyzer.

## Local development notes

- Build this project to validate generator and analyzer changes.
- Verify generated output in consumer projects where endpoint attributes are used.
- Keep diagnostics documented in `AnalyzerReleases.Shipped.md` / `AnalyzerReleases.Unshipped.md`.

## Target framework

- .NET Standard 2.0 (analyzer/generator compatibility target)
