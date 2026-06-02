using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Phymnary.SugarPot.AspNetCore.Domain.Exceptions;
using Phymnary.SugarPot.AspNetCore.Domain.MultiTenancy;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors;

public class SetTenantOnSavingInterceptor(ICurrentTenant currentTenant) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default
    )
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, ct);

        foreach (var entry in eventData.Context.ChangeTracker.Entries<IMultiTenant>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(e => e.TenantId).CurrentValue =
                        currentTenant.Id
                        ?? throw new TenantMissingInContextException("Missing tenant id in scope");
                    break;
                case EntityState.Modified:
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                default:
                    break;
            }

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
