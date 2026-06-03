using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Phymnary.SugarPot.AspNetCore.Auditings;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Exceptions;
using Phymnary.SugarPot.AspNetCore.Extensions;
using Phymnary.SugarPot.AspNetCore.Security;

namespace Phymnary.SugarPot.AspNetCore.Interceptors.Trackers;

internal class EntityPropertyChangeTracker<TAuditDbContext, TAudit>(
    TAuditDbContext auditDbContext,
    ICurrentUser currentUser,
    EfAuditingStructure structure,
    AuditingEntityMapper<IPropertyChangeAudit, TAudit> mapper
) : IEntityPropertyChangeTracker
    where TAuditDbContext : DbContext
    where TAudit : class, IPropertyChangeAudit
{
    private class PropertyChangeAuditData : IPropertyChangeAudit
    {
        public required string EntityName { get; init; }

        public required string PropertyName { get; init; }

        public required string TypeName { get; init; }

        public required string EntityId { get; init; }

        public required string OldValue { get; init; }

        public required string NewValue { get; init; }

        public Guid? ModifiedById { get; init; }

        public DateTimeOffset ModifiedAt { get; init; }

        public bool IsDeleted { get; init; }
    }

    private class Context
    {
        public required string EntityId { get; init; }

        public required string EntityName { get; init; }

        public required DateTimeOffset ModifiedAt { get; init; }

        public required EntityPropertyAuditingMetadata Metadata { get; init; }
    }

    public async ValueTask TrackAsync(
        EntityEntry entry,
        DateTimeOffset modifiedAt,
        CancellationToken ct
    )
    {
        var auditable = (IAuditable)entry.Entity;

        var metadata = structure.GetPropertyAuditingMetadata(auditable.GetType());
        if (!metadata.IsAuditEnabled)
            return;

        var changes = TrackModifyProperties(
            new Context
            {
                EntityId = auditable.GetAuditKey(),
                EntityName = structure.TrackBy switch
                {
                    TrackBy.Database => entry.Metadata.GetTableName()
                        ?? throw new DomainNotImplementedException("Table name not found"),
                    TrackBy.Domain => auditable.GetType().Name,
                    _ => throw new NotSupportedException(
                        $"Not support this TrackBy value {structure.TrackBy}"
                    ),
                },
                ModifiedAt = modifiedAt,
                Metadata = metadata,
            },
            entry
        );

        auditDbContext.Set<TAudit>().AddRange(changes);

        if (structure.HasDifferentDbContextForAuditChanges)
        {
            await auditDbContext.SaveChangesAsync(ct);
        }
    }

    private static bool NotEquals(object? val1, object? val2)
    {
        return !Equals(val1, val2);
    }

    private TAudit Map(
        Context context,
        Microsoft.EntityFrameworkCore.Metadata.IProperty propertyMetadata,
        string ownedFrom,
        object? originalValue,
        object? currentValue
    )
    {
        return mapper.Map(
            new PropertyChangeAuditData
            {
                EntityId = context.EntityId,
                EntityName = context.EntityName,
                PropertyName = structure.TrackBy switch
                {
                    TrackBy.Database => propertyMetadata.GetColumnName(),
                    TrackBy.Domain => ownedFrom + propertyMetadata.Name,
                    _ => throw new NotSupportedException(
                        $"Not support this TrackBy value {structure.TrackBy}"
                    ),
                },
                TypeName = propertyMetadata.ClrType.ShortDisplayName(),
                OldValue = JsonSerializer.Serialize(originalValue),
                NewValue = JsonSerializer.Serialize(currentValue),
                ModifiedById = currentUser.Id.NullIfEmpty(),
                ModifiedAt = context.ModifiedAt,
            }
        );
    }

#pragma warning disable EF1001

    private IEnumerable<TAudit> TrackModifyProperties(
        Context context,
        EntityEntry? entry,
        InternalEntityEntry? infrastructure = null,
        string ownedBy = "",
        bool isModify = false
    )
    {
        if (!ownedBy.IsBlank() && isModify && infrastructure is not null)
        {
            EntityEntry? prev = null;
            if (infrastructure.SharedIdentityEntry is { } shared)
            {
                prev = infrastructure
                    .Context.ChangeTracker.Entries()
                    .FirstOrDefault(it => it.Entity == shared.Entity);
            }

            var properties = prev?.Properties ?? entry?.Properties;
            if (properties is not null)
                foreach (
                    var removedOwnedEntityAudit in properties
                        .Select(it => new
                        {
                            it.Metadata,
                            prev?.Properties.First(p => p.Metadata == it.Metadata).OriginalValue,
                            entry?.Properties.First(p => p.Metadata == it.Metadata).CurrentValue,
                        })
                        .Where(property =>
                            context.Metadata.CanAudit(ownedBy + property.Metadata.Name)
                            && NotEquals(property.OriginalValue, property.CurrentValue)
                        )
                        .Select(property =>
                            Map(
                                context,
                                property.Metadata,
                                ownedBy,
                                property.OriginalValue,
                                property.CurrentValue
                            )
                        )
                )
                    yield return removedOwnedEntityAudit;
        }

        if (entry is null)
            yield break;

        foreach (
            var propertyAudit in entry
                .Properties.Where(property =>
                    property.IsModified
                    && context.Metadata.CanAudit(property.Metadata.Name)
                    && NotEquals(property.OriginalValue, property.CurrentValue)
                )
                .Select(property =>
                    Map(
                        context,
                        property.Metadata,
                        ownedBy,
                        property.OriginalValue,
                        property.CurrentValue
                    )
                )
        )
            yield return propertyAudit;

        foreach (
            var ownedEntityAudit in entry
                .References.Where(refEntry => refEntry.TargetEntry?.Metadata.IsOwned() ?? false)
                .SelectMany(refEntry =>
                    TrackModifyProperties(
                        context,
                        refEntry.TargetEntry,
                        refEntry.IsModified ? refEntry.TargetEntry?.GetInfrastructure() : null,
                        ownedBy + refEntry.Metadata.Name + ".",
                        refEntry.IsModified
                    )
                )
        )
            yield return ownedEntityAudit;
    }
}
#pragma warning restore EF1001
