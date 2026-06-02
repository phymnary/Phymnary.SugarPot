namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Repositories;

public interface IRepositoryOptions<TEntity>
    where TEntity : class, IEntity
{
    IValidator<TEntity>? Validator { get; }

    EntityQueryOptions<TEntity>? QueryOptions { get; }

    EntityUpdateOptions<TEntity>? UpdateOptions { get; }
}
