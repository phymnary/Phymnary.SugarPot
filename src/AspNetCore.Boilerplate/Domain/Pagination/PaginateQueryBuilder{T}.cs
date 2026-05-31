using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Boilerplate.Domain.Pagination;

public static class PaginateQueryBuilderExtensions
{
    public static IPaginateStructure<T> AsNoTracking<T>(this IPaginateStructure<T> builder)
        where T : class
    {
        return builder.BuildUp(queryable => queryable.AsNoTracking());
    }
}

internal class PaginateQueryBuilder<T>(
    Func<CancellationToken, Task<int>> countFunc,
    IQueryable<T> queryable,
    Func<CancellationToken, CancellationToken> getRequestAborted
) : IPaginateOrderBuilding<T>, IPaginatePageBuilding<T>, IPaginateStructure<T>
{
    private IQueryable<T> _queryable = queryable;

    public IPaginatePageBuilding<T> OrderBy<TProperty>(
        Expression<Func<T, TProperty>> propertyAccessor
    )
    {
        _queryable = _queryable.OrderBy(propertyAccessor);
        return this;
    }

    public IPaginatePageBuilding<T> OrderByDescending<TProperty>(
        Expression<Func<T, TProperty>> propertyAccessor
    )
    {
        _queryable = _queryable.OrderByDescending(propertyAccessor);
        return this;
    }

    public IPaginateStructure<T> Pick(int perPage, int pageIndex = 0)
    {
        if (pageIndex > 0)
            _queryable = _queryable.Skip((pageIndex - 1) * perPage);

        if (perPage != int.MaxValue)
            _queryable = _queryable.Take(perPage);

        return this;
    }

    public IPaginateStructure<TTarget> Select<TTarget>(Expression<Func<T, TTarget>> selector)
    {
        return new PaginateQueryBuilder<TTarget>(
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

    public IPaginateStructure<T> BuildUp(Func<IQueryable<T>, IQueryable<T>> manipulate)
    {
        _queryable = manipulate(_queryable);
        return this;
    }

    public IAsyncEnumerable<T> Build()
    {
        return _queryable.AsAsyncEnumerable();
    }
}

public interface IPaginateOrderBuilding<T>
{
    IPaginatePageBuilding<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> propertyAccessor);

    IPaginatePageBuilding<T> OrderByDescending<TProperty>(
        Expression<Func<T, TProperty>> propertyAccessor
    );
}

public interface IPaginatePageBuilding<T>
{
    /// <summary>
    /// Set limit and offset of query
    /// </summary>
    /// <param name="perPage">Number of rows returning. If this equals int.MaxValue, will not invoke Take</param>
    /// <param name="pageIndex">Index start from 1 to match UI page index. If this equals 0, will not invoke Skip.</param>
    /// <returns></returns>
    IPaginateStructure<T> Pick(int perPage, int pageIndex = 0);
}

public interface IPaginateStructure<T>
{
    internal IPaginateStructure<T> BuildUp(Func<IQueryable<T>, IQueryable<T>> control);

    IPaginateStructure<TTarget> Select<TTarget>(Expression<Func<T, TTarget>> selector);

    Task<PaginateResult<T>> CountAndBuildAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> Build();
}
