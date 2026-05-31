using System.Linq.Expressions;
using AspNetCore.Boilerplate.Domain;
using AspNetCore.Boilerplate.Domain.Pagination;
using Microsoft.EntityFrameworkCore;
using Phymnary.SugarPot.AspNetCore.Application.Extensions;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Repositories;

public abstract class EfRepository<TDbContext, TEntity>(
    TDbContext dbContext,
    EfRepositoryAddons addons,
    IRepositoryOptions<TEntity> options
) : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    public DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

    private static EntityQueryOptions<TEntity>? _fallbackQueryOptions;
    private readonly EntityQueryOptions<TEntity> _queryOptions =
        options.QueryOptions ?? (_fallbackQueryOptions ??= new EntityQueryOptions<TEntity>());

    private static EntityUpdateOptions<TEntity>? _fallbackUpdateOptions;
    private readonly EntityUpdateOptions<TEntity> _updateOptions =
        options.UpdateOptions ?? (_fallbackUpdateOptions ??= new EntityUpdateOptions<TEntity>());

    public CancellationToken GetRequestAborted(CancellationToken cancellationToken)
    {
        return addons.AbortedProvider.Get(cancellationToken);
    }

    private Task ValidateAsync(TEntity entity, CancellationToken ct)
    {
        return options.Validator?.ValidateAndThrowOnErrorsAsync(
                entity,
                $"Fail when validate {typeof(TEntity).Name}",
                ct
            ) ?? Task.CompletedTask;
    }

    public async Task<TEntity> InsertAsync(
        TEntity entity,
        bool autoSave = true,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);

        await ValidateAsync(entity, ct);
        var inserted = DbSet.Add(entity).Entity;

        if (autoSave)
            await dbContext.SaveChangesAsync(ct);

        return inserted;
    }

    public async Task<TEntity> UpsertAsync(
        TEntity entity,
        Expression<Func<TEntity, bool>> on,
        bool autoSave = true,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        var upsert = await Queryable(canIncludeDetails).FirstOrDefaultAsync(on, ct);

        if (upsert is not null)
        {
            if (_updateOptions.Update is null)
                throw new AspInvalidOperationException(
                    "Must provide update function in EfUpdateOptions"
                );
            _updateOptions.Update(entity, upsert);
            await ValidateAsync(upsert, ct);
        }
        else
        {
            await ValidateAsync(entity, ct);
            upsert = await InsertAsync(entity, false, ct);
        }

        if (autoSave)
            await dbContext.SaveChangesAsync(ct);

        return upsert;
    }

    public async Task<int> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        await ValidateAsync(entity, ct);

        return await dbContext.SaveChangesAsync(ct);
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        return await Queryable(canIncludeDetails).FirstOrDefaultAsync(predicate, ct);
    }

    public async Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        if (predicate == null)
            return await Queryable(canIncludeDetails).ToListAsync(ct);

        return await Queryable(canIncludeDetails).Where(predicate).ToListAsync(ct);
    }

    public async Task<TSelect[]> SelectWhereAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        if (predicate is null)
            return await DbSet.Select(selector).ToArrayAsync(ct);

        return await DbSet.Where(predicate).Select(selector).ToArrayAsync(ct);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        var ct = GetRequestAborted(cancellationToken);
        return await DbSet.AnyAsync(ct);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        return await DbSet.AnyAsync(predicate, ct);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var ct = GetRequestAborted(cancellationToken);
        return await DbSet.CountAsync(ct);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var ct = GetRequestAborted(cancellationToken);
        return await DbSet.CountAsync(predicate, ct);
    }

    public IPaginateOrderBuilding<TEntity> Paginate(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
        bool? canIncludeDetails = null
    )
    {
        var dbSet = canIncludeDetails is { } include ? Queryable(include) : DbSet;
        var queryable = filter is not null ? filter(dbSet) : dbSet;

        return new PaginateQueryBuilder<TEntity>(
            cancellationToken => queryable.CountAsync(cancellationToken),
            queryable,
            GetRequestAborted
        );
    }

    public IQueryable<TEntity> Queryable(bool canIncludeDetails = false)
    {
        var queryable = _queryOptions.DefaultIncludeQuery.Invoke(DbSet);
        if (canIncludeDetails)
            queryable = _queryOptions.IncludeDetailsQuery.Invoke(queryable);

        return queryable;
    }

    public void Delete(TEntity entity)
    {
        if (_updateOptions.OnDelete is not null)
        {
            _updateOptions.OnDelete(entity);
        }

        if (entity is ISoftDelete softDelete)
        {
            softDelete.SoftDeleted();
        }
        else
        {
            dbContext.Remove(entity);
        }
    }
}
