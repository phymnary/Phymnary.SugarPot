namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Repositories;

public class EntityUpdateOptions<TEntity>
    where TEntity : class, IEntity
{
    public Action<TEntity, TEntity>? Update { get; init; }

    public Action<TEntity>? OnDelete { get; init; }
}
