namespace Phymnary.SugarPot.AspNetCore.Entities;

public interface IEntityValidator<TEntity>
    where TEntity : class, IEntity
{
    ValueTask<EntityValidationResult> ValidateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    );
}
