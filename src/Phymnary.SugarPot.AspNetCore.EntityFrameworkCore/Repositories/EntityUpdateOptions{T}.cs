namespace Phymnary.SugarPot.AspNetCore.Repositories;

public class EntityUpdateOptions<TEntity>
    where TEntity : class, IEntity
{
    public Action<TEntity, TEntity>? Update { get; init; }

    public Action<TEntity>? OnDelete { get; init; }
}
