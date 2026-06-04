# Phymnary.SugarPot.AspNetCore.EntityFrameworkCore

Entity Framework Core integration utilities for Phymnary SugarPot.

This project provides repository base classes, save-change interceptors, and service configuration helpers to standardize common EF Core behaviors such as auditing, soft delete, and multi-tenancy.

## Features

- `EfRepository<TDbContext, TEntity>` and `EfRepository<TDbContext, TEntity, TKey>` base repositories
- Query and update options via repository options (`IRepositoryOptions<T>`)
- Fluent setup through `AddEfCoreServices<TDbContext>()`
- Built-in save interceptors:
  - `AuditOnSavingInterceptor`
  - `SoftDeleteInterceptor`
  - `SetTenantOnSavingInterceptor`
- Property-change audit tracking support (`IEntityPropertyChangeTracker`)
- Model builder helper extensions for default table mapping and query filters
- Support helpers like `DbFunctionProvider` and `WrappedDbContextTransaction`

## Installation

Add a project/package reference to `Phymnary.SugarPot.AspNetCore.EntityFrameworkCore`.

## Service Registration

### Default registration

```csharp
using Phymnary.SugarPot.AspNetCore.Extensions;

services.AddEfCoreServices<AppDbContext>();
```

### Customized registration

```csharp
services.AddEfCoreServices<AppDbContext>(config =>
{
    config.AddSoftDelete();
    config.AddMultiTenancy();

    config.ConfigureAuditing(audit =>
    {
        // configure auditing metadata here
    });

    config.AddPropertyChangeAudit<AuditDbContext, AuditLog>(change => new AuditLog
    {
        // map IPropertyChangeAudit to your audit entity
    });
});
```

## Repository Base Usage

Create a repository by inheriting `EfRepository`:

```csharp
using Microsoft.EntityFrameworkCore;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Repositories;

public sealed class UserRepository(
    AppDbContext dbContext,
    IRepositoryOptions<User> options,
    EfRepositoryAddons addons
) : EfRepository<AppDbContext, User, Guid>(dbContext, options, addons)
{
}
```

Available operations include:

- `InsertAsync`, `UpsertAsync`, `UpdateAsync`
- `FindAsync`, `QueryAsync`
- `AnyAsync`, `CountAsync`
- `AdvanceQuery(...)`
- `Delete(...)`
- `GetAsync(id)` for keyed repositories

## Model Configuration Helper

Use model builder extensions to standardize mapping and global filters:

```csharp
modelBuilder.BuildEntity<User>(schema: "app");
```

`BuildEntity` applies:

- table name = entity type name
- soft-delete filter when entity implements `ISoftDelete`
- tenant filter when entity implements `IMultiTenant` and tenant accessor is provided

## Notes

- `SoftDeleteInterceptor` marks deletion metadata for entities implementing `ISoftDelete`.
- `SetTenantOnSavingInterceptor` enforces tenant assignment for `IMultiTenant` entities.
- `AuditOnSavingInterceptor` updates audit fields and tracks property changes when enabled.

## Target Frameworks

- .NET Standard 2.1