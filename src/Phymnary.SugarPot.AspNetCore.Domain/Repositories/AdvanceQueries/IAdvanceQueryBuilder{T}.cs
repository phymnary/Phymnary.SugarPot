using System.Linq.Expressions;

namespace Phymnary.SugarPot.AspNetCore.Repositories.AdvanceQueries;

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
    IAdvanceSelectableBuilding<T> Pick(int perPage, int pageIndex = 0);
}

public interface IAdvanceSelectableBuilding<T> : IAdvanceQueryBuilder<T>
{
    IAdvanceQueryBuilder<TTarget> Select<TTarget>(Expression<Func<T, TTarget>> selector);
}

public interface IAdvanceQueryBuilder<T>
{
    Task<PaginateResult<T>> PaginateAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> Build();
}
