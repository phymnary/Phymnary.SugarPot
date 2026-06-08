# Phymnary.SugarPot.AspNetCore.Domain

Domain contracts and primitives shared across the SugarPot ASP.NET Core stacks.

This package contains entity abstractions, repository contracts, validation contracts, auditing and multi-tenancy interfaces, runtime context contracts, and domain exception interfaces. It is intentionally framework-agnostic — concrete implementations belong in infrastructure packages.

## What this package provides

- Entity model primitives:
  - `IEntity`
  - `Entity<TKey>`
  - `EntityDomainStatus`
  - `ISoftDelete`
- Repository contracts:
  - `IRepository<TEntity>`
  - `IRepository<TEntity, TKey>`
  - advanced query interfaces in `Repositories.AdvanceQueries`
  - transaction abstraction in `IQueryTransaction`
- Validation contracts:
  - `IEntityValidator<TEntity>`
  - `EntityValidationResult`
  - `EntityValidationFailureDetail`
- Auditing contracts and metadata helpers:
  - `IAuditable`
  - `AuditingAttribute`
  - `DisabledAuditingAttribute`
  - `PropertyChangeAudit`
- Multi-tenancy contracts:
  - `IMultiTenant`
  - `ICurrentTenant`
- Runtime/context contracts:
  - `ICurrentUser`
  - `IRunAt`
  - `IAbortedToken`
  - `IScopeBuilder`
  - `IDbFunctionProvider`
- Domain exception contracts:
  - `IDomainException`
  - `IBusinessException`
  - domain exception types under `Exceptions`

## Installation

`dotnet add package Phymnary.SugarPot.AspNetCore.Domain`

## Quick usage

### Entity model

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

### Repository contract

```csharp
using Phymnary.SugarPot.AspNetCore.Repositories;

public sealed class UserService(IRepository<User, Guid> users)
{
    public Task<User> GetAsync(Guid id, CancellationToken cancellationToken)
        => users.GetAsync(id, cancellationToken: cancellationToken);

    public Task<List<User>> SearchAsync(string keyword, CancellationToken cancellationToken)
        => users.QueryAsync(
            x => x.Name.Contains(keyword),
            cancellationToken: cancellationToken
        );
}
```

### Entity validation contract

```csharp
using Phymnary.SugarPot.AspNetCore.Entities;

public sealed class UserValidator : IEntityValidator<User>
{
    public ValueTask<EntityValidationResult> ValidateAsync(
        User entity,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            return ValueTask.FromResult(
                new EntityValidationResult
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
                }
            );
        }

        return ValueTask.FromResult(
            new EntityValidationResult
            {
                IsValid = true,
                Errors = []
            }
        );
    }
}
```

## Layering guidance

- Keep this package for domain contracts and types only.
- Implement repositories, query providers, transactions, and resilient strategies in infrastructure packages.
- Provide `ICurrentUser`, `ICurrentTenant`, `IRunAt`, and `IAbortedToken` implementations at runtime (for example, via ASP.NET Core host/infrastructure layers).

## Target frameworks

This library targets multiple frameworks to support a range of consumers:

- net8.0
- net9.0
- net10.0

Check the project file for exact target monikers if you need to add or change supported TFMs.

## Contributing

Contributions, bug reports and feature requests are welcome. Please open issues or pull requests on the repository.

## License

See the repository root for license information.
