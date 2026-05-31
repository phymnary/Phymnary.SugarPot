using AspNetCore.Boilerplate.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Phymnary.SugarPot.AspNetCore.Api;
using Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors;

public class SoftDeleteInterceptor(ICurrentUser currentUser, RequestedAt requestedAt)
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

        var at = requestedAt.Value;
        foreach (var entry in dbContext.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.Entity.DeletedAt != DateTimeOffset.MaxValue)
                continue;

            entry.Entity.DeletedAt = at;
            entry.Entity.DeletedById = currentUser.Id;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
