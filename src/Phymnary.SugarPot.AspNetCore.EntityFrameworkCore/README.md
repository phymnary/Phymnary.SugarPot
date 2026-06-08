# Phymnary.SugarPot.AspNetCore.EntityFrameworkCore

Entity Framework Core integration helpers and repository support for Phymnary SugarPot.

This project provides EF Core repository base classes, save-change interceptors, model builder helpers, and service registration helpers to standardize common behaviors such as auditing, soft delete, multi-tenancy, and property-change auditing.

## Features

- EfRepository<TDbContext, TEntity> and EfRepository<TDbContext, TEntity, TKey> base repositories
- Query and update options via repository options (IRepositoryOptions<T>)
- Fluent setup through AddEfCoreServices<TDbContext>()
- Built-in save interceptors:
  - AuditOnSavingInterceptor
  - SoftDeleteInterceptor
  - SetTenantOnSavingInterceptor
- Property-change audit tracking support (IEntityPropertyChangeTracker)
- Model builder helper extensions for default table mapping and query filters
- Supporting helpers like DbFunctionProvider and WrappedDbContextTransaction

## Installation

`dotnet add package Phymnary.SugarPot.AspNetCore.EntityFrameworkCore`

### Note about EF Core versions

The project file sets the EF Core package version range based on the target TFM: when targeting net10.0 it prefers EF Core 10.x; for earlier TFMs it targets EF Core 8.x+. See the package's .csproj for details.

## Service Registration

Default registration:

```csharp
using Phymnary.SugarPot.AspNetCore.Extensions;

services.AddEfCoreServices<AppDbContext>();
```

Customized registration:

```csharp
services
    .AddSoftDelete();
    .AddMultiTenancy()
    .AddEfCoreServices<AppDbContext>(config =>
    {
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

Create a repository by inheriting EfRepository:

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

Common operations include `InsertAsync`, `UpsertAsync`, `UpdateAsync`, `FindAsync`, `QueryAsync`, `AnyAsync`, `CountAsync`, `AdvanceQuery(...)`, `Delete(...)`, and `GetAsync(id)` for keyed repositories.

## Model Configuration Helper

Use model builder extensions to standardize mapping and global filters:

```csharp
modelBuilder.BuildEntity<User>(schema: "app");
```

`BuildEntity` applies table naming, soft-delete filters for `ISoftDelete`, and tenant filters for `IMultiTenant` when tenant accessor is available.

## Notes

- `SoftDeleteInterceptor` marks deletion metadata for entities implementing `ISoftDelete`.
- `SetTenantOnSavingInterceptor` enforces tenant assignment for `IMultiTenant` entities.
- `AuditOnSavingInterceptor` updates audit fields and tracks property changes when enabled.

## Target frameworks

This library is built to support a range of TFMs used in the solution. Typical target frameworks include:

- net8.0
- net9.0
- net10.0

Check the project file if you need the exact TFMs and EF Core versioning behavior.

## Contributing

Contributions, bug reports and feature requests are welcome. Please open issues or pull requests on the repository.

## License

See the repository root for license information.