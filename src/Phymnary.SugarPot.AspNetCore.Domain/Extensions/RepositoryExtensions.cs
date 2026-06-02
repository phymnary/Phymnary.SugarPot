using Microsoft.EntityFrameworkCore;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Exceptions;
using Phymnary.SugarPot.AspNetCore.Repositories;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class RepositoryExtensions
{
    public static async Task<TEntity> GetAsync<TEntity, TId>(
        this IRepository<TEntity> repository,
        TId id,
        bool canIncludeDetails = false,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        return await repository
                .Queryable(canIncludeDetails)
                .FirstOrDefaultAsync(
                    entity => entity.Id.Equals(id),
                    repository.GetRequestAborted(cancellationToken)
                )
            ?? throw new EntityNotFoundException(
                $"Could not find entity {typeof(TEntity).Name} with id {id}"
            );
    }
}
