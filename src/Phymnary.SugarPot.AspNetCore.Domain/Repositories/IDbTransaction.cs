namespace Phymnary.SugarPot.AspNetCore.Repositories;

public interface IQueryTransaction : IDisposable, IAsyncDisposable
{
    //
    // Summary:
    //     Gets the transaction identifier.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    Guid TransactionId { get; }

    //
    // Summary:
    //     Gets a value that indicates whether this Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction
    //     instance supports database savepoints. If false, the methods Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.CreateSavepointAsync(System.String,System.Threading.CancellationToken),
    //     Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.RollbackToSavepointAsync(System.String,System.Threading.CancellationToken)
    //     and Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.ReleaseSavepointAsync(System.String,System.Threading.CancellationToken)
    //     as well as their synchronous counterparts are expected to throw System.NotSupportedException.
    //
    //
    // Returns:
    //     true if this Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction instance
    //     supports database savepoints; otherwise, false.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    bool SupportsSavepoints => false;

    //
    // Summary:
    //     Commits all changes made to the database in the current transaction.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    void Commit();

    //
    // Summary:
    //     Commits all changes made to the database in the current transaction asynchronously.
    //
    //
    // Parameters:
    //   cancellationToken:
    //     A System.Threading.CancellationToken to observe while waiting for the task to
    //     complete.
    //
    // Returns:
    //     A System.Threading.Tasks.Task representing the asynchronous operation.
    //
    // Exceptions:
    //   T:System.OperationCanceledException:
    //     If the System.Threading.CancellationToken is canceled.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    Task CommitAsync(CancellationToken cancellationToken = default);

    //
    // Summary:
    //     Discards all changes made to the database in the current transaction.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    void Rollback();

    //
    // Summary:
    //     Discards all changes made to the database in the current transaction asynchronously.
    //
    //
    // Parameters:
    //   cancellationToken:
    //     A System.Threading.CancellationToken to observe while waiting for the task to
    //     complete.
    //
    // Returns:
    //     A System.Threading.Tasks.Task representing the asynchronous operation.
    //
    // Exceptions:
    //   T:System.OperationCanceledException:
    //     If the System.Threading.CancellationToken is canceled.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    Task RollbackAsync(CancellationToken cancellationToken = default);

    //
    // Summary:
    //     Creates a savepoint in the transaction. This allows all commands that are executed
    //     after the savepoint was established to be rolled back, restoring the transaction
    //     state to what it was at the time of the savepoint.
    //
    // Parameters:
    //   name:
    //     The name of the savepoint to be created.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    void CreateSavepoint(string name);

    //
    // Summary:
    //     Creates a savepoint in the transaction. This allows all commands that are executed
    //     after the savepoint was established to be rolled back, restoring the transaction
    //     state to what it was at the time of the savepoint.
    //
    // Parameters:
    //   name:
    //     The name of the savepoint to be created.
    //
    //   cancellationToken:
    //     A System.Threading.CancellationToken to observe while waiting for the task to
    //     complete.
    //
    // Returns:
    //     A System.Threading.Tasks.Task representing the asynchronous operation.
    //
    // Exceptions:
    //   T:System.OperationCanceledException:
    //     If the System.Threading.CancellationToken is canceled.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    Task CreateSavepointAsync(string name, CancellationToken cancellationToken = default);

    //
    // Summary:
    //     Rolls back all commands that were executed after the specified savepoint was
    //     established.
    //
    // Parameters:
    //   name:
    //     The name of the savepoint to roll back to.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    void RollbackToSavepoint(string name);

    //
    // Summary:
    //     Rolls back all commands that were executed after the specified savepoint was
    //     established.
    //
    // Parameters:
    //   name:
    //     The name of the savepoint to roll back to.
    //
    //   cancellationToken:
    //     A System.Threading.CancellationToken to observe while waiting for the task to
    //     complete.
    //
    // Returns:
    //     A System.Threading.Tasks.Task representing the asynchronous operation.
    //
    // Exceptions:
    //   T:System.OperationCanceledException:
    //     If the System.Threading.CancellationToken is canceled.
    //
    // Remarks:
    //     See Transactions in EF Core for more information and examples.
    Task RollbackToSavepointAsync(string name, CancellationToken cancellationToken = default);

    //
    // Summary:
    //     Destroys a savepoint previously defined in the current transaction. This allows
    //     the system to reclaim some resources before the transaction ends.
    //
    // Parameters:
    //   name:
    //     The name of the savepoint to release.
    //
    // Remarks:
    //     If savepoint release isn't supported, Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.ReleaseSavepoint(System.String)
    //     and Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.ReleaseSavepointAsync(System.String,System.Threading.CancellationToken)
    //     should do nothing rather than throw. This is the default behavior.
    //
    //     See Transactions in EF Core for more information and examples.
    void ReleaseSavepoint(string name) { }

    //
    // Summary:
    //     Destroys a savepoint previously defined in the current transaction. This allows
    //     the system to reclaim some resources before the transaction ends.
    //
    // Parameters:
    //   name:
    //     The name of the savepoint to release.
    //
    //   cancellationToken:
    //     A System.Threading.CancellationToken to observe while waiting for the task to
    //     complete.
    //
    // Returns:
    //     A System.Threading.Tasks.Task representing the asynchronous operation.
    //
    // Exceptions:
    //   T:System.OperationCanceledException:
    //     If the System.Threading.CancellationToken is canceled.
    //
    // Remarks:
    //     If savepoint release isn't supported, Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.ReleaseSavepoint(System.String)
    //     and Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.ReleaseSavepointAsync(System.String,System.Threading.CancellationToken)
    //     should do nothing rather than throw. This is the default behavior.
    //
    //     See Transactions in EF Core for more information and examples.
    Task ReleaseSavepointAsync(string name, CancellationToken cancellationToken = default);
}
