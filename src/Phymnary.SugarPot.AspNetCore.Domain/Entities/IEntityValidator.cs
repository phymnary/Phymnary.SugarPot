using FluentValidation.Results;

namespace Phymnary.SugarPot.AspNetCore.Domain.Entities;

public interface IEntityValidator<TEntity>
    where TEntity : class, IEntity
{
    ValueTask<ValidationResult> ValidateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    );
}
