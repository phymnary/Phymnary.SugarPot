using Microsoft.EntityFrameworkCore.Diagnostics;
using Phymnary.SugarPot.AspNetCore.Domain;
using Phymnary.SugarPot.AspNetCore.Domain.Entities;
using Phymnary.SugarPot.AspNetCore.Domain.Security;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors;

public class SoftDeleteInterceptor(ICurrentUser currentUser, IRequestedAt requestedAt)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context is not { } dbContext)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in dbContext.ChangeTracker.Entries<ISoftDelete>())
        {
            if (!entry.Entity.DomainStatus.IsSoftDeleted)
                continue;

            entry.Entity.DeletedAt = requestedAt.Value;
            entry.Entity.DeletedById = currentUser.Id;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
