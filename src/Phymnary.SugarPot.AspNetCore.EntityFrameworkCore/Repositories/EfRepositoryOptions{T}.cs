using Phymnary.SugarPot.AspNetCore.Entities;

namespace Phymnary.SugarPot.AspNetCore.Repositories;

public abstract class EfRepositoryOptions<TEntity> : IRepositoryOptions<TEntity>
    where TEntity : class, IEntity
{
    public IEntityValidator<TEntity>? Validator { get; protected init; }

    public EntityQueryOptions<TEntity>? QueryOptions { get; protected init; }

    public EntityUpdateOptions<TEntity>? UpdateOptions { get; protected init; }
}
