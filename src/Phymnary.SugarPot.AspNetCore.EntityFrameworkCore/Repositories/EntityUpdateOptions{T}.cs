using Phymnary.SugarPot.AspNetCore.Entities;

namespace Phymnary.SugarPot.AspNetCore.Repositories;

public class EntityUpdateOptions<TEntity>
    where TEntity : IEntity
{
    public Action<TEntity, TEntity>? Update { get; init; }

    public Func<TEntity, bool>? OnDelete { get; init; }
}
