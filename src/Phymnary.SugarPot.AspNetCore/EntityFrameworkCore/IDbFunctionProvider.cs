using Microsoft.EntityFrameworkCore.Storage;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

public interface IDbFunctionProvider
{
    Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );

    Task UseResilientStrategy(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default
    );

    Task UseResilientStrategyWithTransaction(
        Func<CancellationToken, Task> operation,
        Func<CancellationToken, Task<bool>> verifySucceeded,
        CancellationToken cancellationToken = default
    );

    void ClearTracking();
}
