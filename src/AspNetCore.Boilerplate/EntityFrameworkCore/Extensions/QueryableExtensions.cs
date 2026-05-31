using System.Linq.Expressions;
using AspNetCore.Boilerplate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AspNetCore.Boilerplate.EntityFrameworkCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> IncludeThen<TEntity, TProperty>(
        this IQueryable<TEntity> source,
        Expression<Func<TEntity, TProperty>> navigationPropertyPath,
        params Func<IIncludableQueryable<TEntity, TProperty>, IQueryable<TEntity>>[] thenIncludes
    )
        where TEntity : class, IEntity
    {
        source = source.Include(navigationPropertyPath);

        return thenIncludes.Aggregate(
            source,
            (current, thenInclude) => thenInclude(current.Include(navigationPropertyPath))
        );
    }
}
