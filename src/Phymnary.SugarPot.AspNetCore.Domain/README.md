# Phymnary.SugarPot.AspNetCore.Domain

Core domain contracts and abstractions for SugarPot ASP.NET Core projects.

This project defines the base entity model, repository contracts, auditing interfaces, multi-tenancy contracts, validation primitives, and domain exception types used across the solution.

## Features

- Base entity abstractions:
  - `IEntity`
  - `Entity<TKey>`
  - `EntityDomainStatus`
- Repository contracts:
  - `IRepository<TEntity>`
  - `IRepository<TEntity, TKey>`
  - advanced query interfaces under `Repositories.AdvanceQueries`
- Domain validation contracts:
  - `IEntityValidator<TEntity>`
  - `EntityValidationResult` and failure details
- Auditing contracts:
  - `IAuditable`
  - property change audit models and attributes
- Multi-tenancy contracts:
  - `IMultiTenant`
  - `ICurrentTenant`
- Request/runtime abstractions:
  - `ICurrentUser`
  - `IRunAt`
  - `IAbortedToken`
- Domain exceptions and business exception interfaces

## Installation

Add a project/package reference to `Phymnary.SugarPot.AspNetCore.Domain`.

## Entity Model Example

```csharp
using Phymnary.SugarPot.AspNetCore.Entities;

public sealed class User : Entity<Guid>
{
    public User(Guid id) : base(id)
    {
    }

    public string Name { get; set; } = string.Empty;
}
```

## Repository Contract Example

```csharp
using System.Linq.Expressions;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Repositories;

public sealed class UserService(IRepository<User, Guid> users)
{
    public Task<User> GetAsync(Guid id, CancellationToken ct)
        => users.GetAsync(id, cancellationToken: ct);

    public Task<List<User>> QueryAsync(Expression<Func<User, bool>> predicate, CancellationToken ct)
        => users.QueryAsync(predicate, cancellationToken: ct);
}
```

## Validation Example

```csharp
using Phymnary.SugarPot.AspNetCore.Entities;

public sealed class UserValidator : IEntityValidator<User>
{
    public ValueTask<EntityValidationResult> ValidateAsync(User entity, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            return ValueTask.FromResult(new EntityValidationResult
            {
                IsValid = false,
                Errors =
                [
                    new EntityValidationFailureDetail
                    {
                        Property = nameof(User.Name),
                        Message = "Name is required"
                    }
                ]
            });
        }

        return ValueTask.FromResult(EntityValidationResult.Valid);
    }
}
```

## Auditing and Multi-Tenancy Contracts

Implement these interfaces on entities when needed:

- `IAuditable` for created/updated metadata
- `IMultiTenant` for tenant ownership

Use runtime providers from application/infrastructure layers:

- `ICurrentUser` for current user id
- `ICurrentTenant` for current tenant id
- `IRunAt` for current time abstraction

## Notes

- This project contains contracts and domain-level primitives only.
- Data access implementations are provided by infrastructure projects (for example, Entity Framework Core integration).

## Target Frameworks

- .NET Standard 2.0
- .NET 8
