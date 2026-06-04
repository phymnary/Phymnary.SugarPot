using System.Linq.Expressions;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Exceptions;
using Phymnary.SugarPot.AspNetCore.Repositories.AdvanceQueries;

namespace Phymnary.SugarPot.AspNetCore.Repositories;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Insert entity to database context
    /// </summary>
    /// <param name="entity">Entity to be inserted.</param>
    /// <param name="autoSave">Call SaveChanges or not.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">Database exceptions.</exception>
    /// <exception cref="EntityValidationException">Throw by validator, run before entity got added to DbContext so failed entity won't populate EF tracker.</exception>
    Task<TEntity> InsertAsync(
        TEntity entity,
        bool autoSave = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Insert entity to database context
    /// </summary>
    /// <param name="entity">Entity to be inserted or updated if existed.</param>
    /// <param name="on">Expression to check if entity is existed or not</param>
    /// <param name="autoSave">Call SaveChanges or not.</param>
    /// <param name="canIncludeDetails">Call IncludeDetails.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">Database exceptions.</exception>
    /// <exception cref="EntityValidationException">Throw by validator.</exception>
    Task<TEntity> UpsertAsync(
        TEntity entity,
        Expression<Func<TEntity, bool>> on,
        bool autoSave = true,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    );

    ///<summary>
    /// Validate entity then SaveChanges
    /// </summary>
    /// <param name="entity">Entity to be updated.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">Database exceptions.</exception>
    /// <exception cref="EntityValidationException">Throw by validator.</exception>
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find the first entity matching the specified predicate.
    /// </summary>
    /// <param name="predicate">Predicate used to filter entities.</param>
    /// <param name="canIncludeDetails">Call IncludeDetails.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Query entities using an optional predicate.
    /// </summary>
    /// <param name="predicate">Optional predicate used to filter entities.</param>
    /// <param name="canIncludeDetails">Call IncludeDetails.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Check whether any entity exists.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check whether any entity matches the specified predicate.
    /// </summary>
    /// <param name="predicate">Predicate used to filter entities.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Count all entities.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Count entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">Predicate used to filter entities.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Start building an advanced query.
    /// </summary>
    /// <param name="filter">Optional query filter builder.</param>
    /// <param name="canIncludeDetails">Optional flag to call IncludeDetails.</param>
    IAdvanceOrderBuilding<TEntity> AdvanceQuery(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
        bool? canIncludeDetails = null
    );

    /// <summary>
    /// Mark an entity for deletion.
    /// </summary>
    /// <param name="entity">Entity to be deleted.</param>
    void Delete(TEntity entity);
}

public interface IRepository<TEntity, TKey> : IRepository<TEntity>
    where TEntity : Entity<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// Get entity by key.
    /// </summary>
    /// <param name="id">Entity key value.</param>
    /// <param name="canIncludeDetails">Call IncludeDetails.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The entity with the specified key.</returns>
    /// <exception cref="EntityNotFoundException">Thrown if the entity with the specified key is not found.</exception>
    Task<TEntity> GetAsync(
        TKey id,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    );
}
