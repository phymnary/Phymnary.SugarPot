namespace Phymnary.SugarPot.AspNetCore.Domain.AdvanceQueries;

public class PaginateResult<TEntity>
{
    public required int Count { get; init; }

    public required IAsyncEnumerable<TEntity> Items { get; init; }
}
