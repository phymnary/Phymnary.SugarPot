using System.Collections.Frozen;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Extensions;

namespace Phymnary.SugarPot.AspNetCore.Auditings;

/// <summary>
/// Singleton service to store the auditing structure of application. It also caches the auditing metadata for each entity type.
/// </summary>
public class EfAuditingStructure
{
    private readonly Dictionary<
        Type,
        EntityPropertyAuditingMetadata
    > _cachePropertyAuditingMetadata = [];

    public TrackBy TrackBy { internal get; set; }

    internal bool HasDifferentDbContextForAuditChanges { get; set; }

    private static IEnumerable<string> GetDisabledAuditPropertyNames(
        IEnumerable<PropertyInfo> propertyInfos,
        string? ownedBy = null
    )
    {
        var owned = ownedBy.TryGetValuable(out var parentName) ? parentName + "." : string.Empty;

        foreach (var propertyInfo in propertyInfos)
        {
            var type = propertyInfo.PropertyType;
            if (type.IsClass)
            {
                if (type.HasAttribute<OwnedAttribute>())
                {
                    foreach (
                        var disabled in GetDisabledAuditPropertyNames(
                            type.GetProperties(),
                            owned + propertyInfo.Name
                        )
                    )
                    {
                        yield return disabled;
                    }
                }
                else if (propertyInfo.HasAttribute<DisableAuditingAttribute>())
                {
                    if (type.IsAssignableTo(typeof(IEntity)))
                    {
                        yield return owned + propertyInfo.Name + "Id";
                    }
                    if (type == typeof(string))
                        yield return owned + propertyInfo.Name;
                }
                continue;
            }

            if (propertyInfo.HasAttribute<DisableAuditingAttribute>())
                yield return owned + propertyInfo.Name;
        }
    }

    private static readonly FrozenSet<string> EmptyFrozenSet = FrozenSet.ToFrozenSet<string>([]);

    internal EntityPropertyAuditingMetadata GetPropertyAuditingMetadata(Type entityType)
    {
        if (_cachePropertyAuditingMetadata.TryGetValue(entityType, out var value))
            return value;

        value = new EntityPropertyAuditingMetadata
        {
            IsAuditEnabled = !entityType.HasAttribute<DisableAuditingAttribute>(),
            ValidAuditProperties =
                entityType.GetCustomAttribute<AuditedAttribute>()?.Properties.ToFrozenSet()
                ?? EmptyFrozenSet,
            IgnoreAuditProperties = FrozenSet.ToFrozenSet([
                "CreatedAt",
                "CreatedById",
                "UpdatedAt",
                "UpdatedById",
                "TenantId",
                .. GetDisabledAuditPropertyNames(entityType.GetProperties()),
            ]),
        };

        _cachePropertyAuditingMetadata[entityType] = value;

        return value;
    }
}
