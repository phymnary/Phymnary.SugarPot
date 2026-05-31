using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

public class DbFunctionProvider<TDbContext>(TDbContext dbContext, IAbortedProvider ctProvider)
    : IDbFunctionProvider
    where TDbContext : DbContext
{
    public async Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    )
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(
            ctProvider.Get(cancellationToken)
        );
        return new ProviderDbContextTransaction(transaction, ctProvider);
    }

    public async Task UseResilientStrategy(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(
            async ct =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
                await operation(ct);
                await transaction.CommitAsync(ct);
            },
            ctProvider.Get(cancellationToken)
        );
    }

    public async Task UseResilientStrategyWithTransaction(
        Func<CancellationToken, Task> operation,
        Func<CancellationToken, Task<bool>> verifySucceeded,
        CancellationToken cancellationToken = default
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteInTransactionAsync(
            async ct =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
                await operation(ct);
                await transaction.CommitAsync(ct);
            },
            verifySucceeded,
            ctProvider.Get(cancellationToken)
        );
    }

    public void ClearTracking()
    {
        dbContext.ChangeTracker.Clear();
    }
}
