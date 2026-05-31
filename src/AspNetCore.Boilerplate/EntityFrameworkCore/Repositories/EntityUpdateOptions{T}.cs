using AspNetCore.Boilerplate.Domain;

namespace AspNetCore.Boilerplate.EntityFrameworkCore.Repositories;

public class EntityUpdateOptions<TEntity>
    where TEntity : class, IEntity
{
    public Action<TEntity, TEntity>? Update { get; init; }

    public Action<TEntity>? OnDelete { get; init; }
}
