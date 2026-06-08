# Phymnary.SugarPot

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A modular ASP.NET Core framework providing opinionated building blocks for domain-driven applications — including repository patterns, entity auditing, multi-tenancy, soft delete, and structured exception handling.

## Features

- **Repository Pattern** — Generic `IRepository<TEntity>` with CRUD, upsert, advanced query building, and pagination
- **Entity Framework Core Integration** — Interceptor-based auditing, soft delete, and multi-tenancy
- **Domain Abstractions** — `Entity<TKey>`, `ISoftDelete`, `IAuditable`, `IMultiTenant`
- **Structured Exception Handling** — `IBusinessException` with automatic JSON error responses
- **Entity Validation** — Domain-level validation before persistence via `IEntityValidator<T>`
- **Configuration Binding** — Strongly-typed `IAppSettingsSection` with `IValidateOptions` support
- **Multi-Tenancy** — Automatic tenant assignment via EF Core interceptors
- **Scoped Execution** — `IScopeBuilder` for initializing background service scopes with user/tenant context
- **Roslyn Source Generators** — Compile-time code generation for boilerplate reduction

## Packages

| Package | Description |
|---------|-------------|
| `Phymnary.SugarPot.AspNetCore.Domain` | Domain abstractions — entities, repositories, exceptions, multi-tenancy |
| `Phymnary.SugarPot.AspNetCore.Api` | API-layer services — exception handler, current user/tenant from HttpContext |
| `Phymnary.SugarPot.AspNetCore.Application` | Application-layer services — timestamps, runtime context |
| `Phymnary.SugarPot.AspNetCore.EntityFrameworkCore` | EF Core repository implementation with interceptors |
| `Phymnary.SugarPot.Module` | Module configuration utilities with section binding |
| `Phymnary.SugarPot.RoslynComponents` | Shared Roslyn component library for source generators |

## Target Frameworks

`net8.0` · `net9.0` · `net10.0`