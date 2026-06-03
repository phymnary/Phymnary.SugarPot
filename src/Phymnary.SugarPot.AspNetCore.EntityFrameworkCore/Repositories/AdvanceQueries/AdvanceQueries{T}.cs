using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Phymnary.SugarPot.AspNetCore.Repositories.AdvanceQueries;

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

    public async Task<PaginateResult<T>> PaginateAsync(
        CancellationToken cancellationToken = default
    )
    {
        return new PaginateResult<T>
        {
            Count = await countFunc(getRequestAborted(cancellationToken)),
            Items = _queryable.AsAsyncEnumerable(),
        };
    }

    public IAsyncEnumerable<T> Build()
    {
        return _queryable.AsAsyncEnumerable();
    }
}
