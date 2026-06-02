using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Phymnary.SugarPot.AspNetCore;

public class DbFunctionProvider<TDbContext>(TDbContext dbContext, IAbortedToken ctProvider)
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
        return new WrappedDbContextTransaction(transaction, ctProvider);
    }

    public async Task UseResilientStrategyAsync(
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

    public async Task UseResilientStrategyWithTransactionAsync(
        Func<CancellationToken, Task> operation,
        Func<CancellationToken, Task<bool>> verifySucceeded,
        CancellationToken cancellationToken = default
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteInTransactionAsync(
            async ct =>
            {
                await operation(ct);
                await dbContext.SaveChangesAsync(acceptAllChangesOnSuccess: false, ct);
            },
            verifySucceeded,
            ctProvider.Get(cancellationToken)
        );
        dbContext.ChangeTracker.AcceptAllChanges();
    }

    public void ClearTracking()
    {
        dbContext.ChangeTracker.Clear();
    }
}
