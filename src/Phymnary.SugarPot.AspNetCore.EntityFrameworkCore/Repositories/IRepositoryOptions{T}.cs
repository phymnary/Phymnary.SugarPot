using Phymnary.SugarPot.AspNetCore.Entities;

namespace Phymnary.SugarPot.AspNetCore.Repositories;

public interface IRepositoryOptions<TEntity>
    where TEntity : class, IEntity
{
    IEntityValidator<TEntity>? Validator { get; }

    EntityQueryOptions<TEntity>? QueryOptions { get; }

    EntityUpdateOptions<TEntity>? UpdateOptions { get; }
}
