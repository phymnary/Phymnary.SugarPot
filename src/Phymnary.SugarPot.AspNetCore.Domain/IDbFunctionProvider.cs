using Microsoft.EntityFrameworkCore.Storage;

namespace Phymnary.SugarPot.AspNetCore;

public interface IDbFunctionProvider
{
    Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );

    Task UseResilientStrategyAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default
    );

    Task UseResilientStrategyWithTransactionAsync(
        Func<CancellationToken, Task> operation,
        Func<CancellationToken, Task<bool>> verifySucceeded,
        CancellationToken cancellationToken = default
    );

    void ClearTracking();
}
