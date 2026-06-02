using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Phymnary.SugarPot.AspNetCore.Entities;
using System.Linq.Expressions;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> IncludeIn<TEntity, TProperty>(
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
