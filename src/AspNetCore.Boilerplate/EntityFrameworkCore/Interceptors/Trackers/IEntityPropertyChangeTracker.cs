using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors.Trackers;

public interface IEntityPropertyChangeTracker
{
    ValueTask TrackAsync(EntityEntry entry, DateTimeOffset modifiedAt, CancellationToken ct);
}
