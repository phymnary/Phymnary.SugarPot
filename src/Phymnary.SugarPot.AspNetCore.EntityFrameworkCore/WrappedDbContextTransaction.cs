using Microsoft.EntityFrameworkCore.Storage;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

internal sealed class WrappedDbContextTransaction(
    IDbContextTransaction transaction,
    IAbortedToken abortedProvider
) : IDbContextTransaction
{
    public Guid TransactionId { get; } = transaction.TransactionId;

    public bool SupportsSavepoints => transaction.SupportsSavepoints;

    public void Commit()
    {
        transaction.Commit();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return transaction.CommitAsync(abortedProvider.Get(cancellationToken));
    }

    public void Rollback()
    {
        transaction.Rollback();
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return transaction.RollbackAsync(abortedProvider.Get(cancellationToken));
    }

    public void CreateSavepoint(string name)
    {
        transaction.CreateSavepoint(name);
    }

    public Task CreateSavepointAsync(string name, CancellationToken cancellationToken = default)
    {
        return transaction.CreateSavepointAsync(name, abortedProvider.Get(cancellationToken));
    }

    public void RollbackToSavepoint(string name)
    {
        transaction.RollbackToSavepoint(name);
    }

    public Task RollbackToSavepointAsync(string name, CancellationToken cancellationToken = default)
    {
        return transaction.RollbackToSavepointAsync(name, abortedProvider.Get(cancellationToken));
    }

    public void ReleaseSavepoint(string name)
    {
        transaction.ReleaseSavepoint(name);
    }

    public Task ReleaseSavepointAsync(string name, CancellationToken cancellationToken = default)
    {
        return transaction.ReleaseSavepointAsync(name, abortedProvider.Get(cancellationToken));
    }

    public void Dispose()
    {
        transaction.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
    }
}
