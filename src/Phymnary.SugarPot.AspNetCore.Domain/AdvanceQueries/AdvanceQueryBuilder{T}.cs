using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Phymnary.SugarPot.AspNetCore.Domain.AdvanceQueries;

namespace Phymnary.SugarPot.AspNetCore.Domain.AdvanceQueries;

public static class AdvanceQueryBuilderExtensions
{
    public static IAdvanceQueryBuilder<T> AsNoTracking<T>(this IAdvanceQueryBuilder<T> builder)
        where T : class
    {
        return builder.BuildUp(queryable => queryable.AsNoTracking());
    }
}

internal class AdvanceQueryBuilder<T>(
    Func<CancellationToken, Task<int>> countFunc,
    IQueryable<T> queryable,
    Func<CancellationToken, CancellationToken> getRequestAborted
) : IAdvanceOrderBuilding<T>, IAdvancePageBuilding<T>, IAdvanceQueryBuilder<T>
{
    private IQueryable<T> _queryable = queryable;

    public IAdvancePageBuilding<T> OrderBy<TProperty>(
        Expression<Func<T, TProperty>> propertyAccessor
    )
    {
        _queryable = _queryable.OrderBy(propertyAccessor);
        return this;
    }

    public IAdvancePageBuilding<T> OrderByDescending<TProperty>(
        Expression<Func<T, TProperty>> propertyAccessor
    )
    {
        _queryable = _queryable.OrderByDescending(propertyAccessor);
        return this;
    }

    public IAdvanceQueryBuilder<T> Pick(int perPage, int pageIndex = 0)
    {
        if (pageIndex > 0)
            _queryable = _queryable.Skip((pageIndex - 1) * perPage);

        if (perPage != int.MaxValue)
            _queryable = _queryable.Take(perPage);

        return this;
    }

    public IAdvanceQueryBuilder<TTarget> Select<TTarget>(Expression<Func<T, TTarget>> selector)
    {
        return new AdvanceQueryBuilder<TTarget>(
            countFunc,
            _queryable.Select(selector),
            getRequestAborted
        );
    }

    public async Task<PaginateResult<T>> CountAndBuildAsync(
        CancellationToken cancellationToken = default
    )
    {
        return new PaginateResult<T>
        {
            Count = await countFunc(getRequestAborted(cancellationToken)),
            Items = _queryable.AsAsyncEnumerable(),
        };
    }

    public IAdvanceQueryBuilder<T> BuildUp(Func<IQueryable<T>, IQueryable<T>> manipulate)
    {
        _queryable = manipulate(_queryable);
        return this;
    }

    public IAsyncEnumerable<T> Build()
    {
        return _queryable.AsAsyncEnumerable();
    }
}

public interface IAdvanceOrderBuilding<T>
{
    IAdvancePageBuilding<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> propertyAccessor);

    IAdvancePageBuilding<T> OrderByDescending<TProperty>(
        Expression<Func<T, TProperty>> propertyAccessor
    );
}

public interface IAdvancePageBuilding<T>
{
    /// <summary>
    /// Set limit and offset of query
    /// </summary>
    /// <param name="perPage">Number of rows returning. If this equals int.MaxValue, will not invoke Take</param>
    /// <param name="pageIndex">Index start from 1 to match UI page index. If this equals 0, will not invoke Skip.</param>
    /// <returns></returns>
    IAdvanceQueryBuilder<T> Pick(int perPage, int pageIndex = 0);
}

public interface IAdvanceQueryBuilder<T>
{
    internal IAdvanceQueryBuilder<T> BuildUp(Func<IQueryable<T>, IQueryable<T>> control);

    IAdvanceQueryBuilder<TTarget> Select<TTarget>(Expression<Func<T, TTarget>> selector);

    Task<PaginateResult<T>> CountAndBuildAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> Build();
}
