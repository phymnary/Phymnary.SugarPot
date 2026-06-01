using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors.Trackers;

public class EmptyEntityPropertyChangeTracker : IEntityPropertyChangeTracker
{
    public ValueTask TrackAsync(EntityEntry entry, DateTimeOffset modifiedAt, CancellationToken ct)
    {
        return default;
    }
}
