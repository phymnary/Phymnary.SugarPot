namespace Phymnary.SugarPot.AspNetCore.Repositories;

public class EntityQueryOptions<TEntity>
    where TEntity : IEntity
{
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> DefaultIncludeQuery { get; init; } =
        queryable => queryable;

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> IncludeDetailsQuery { get; init; } =
        queryable => queryable;
}
