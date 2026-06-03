using System.Collections.Immutable;

namespace Phymnary.SugarPot.AspNetCore.Auditings;

public class EntityPropertyAuditingMetadata
{
    public bool IsAuditEnabled { get; set; }

    public required ImmutableHashSet<string> ValidAuditProperties { private get; init; }

    public required ImmutableHashSet<string> IgnoreAuditProperties { private get; init; }

    public bool CanAudit(string name)
    {
        var isValid = ValidAuditProperties.IsEmpty || ValidAuditProperties.Contains(name);
        var isNotIgnored = !IgnoreAuditProperties.Contains(name);

        return isValid && isNotIgnored;
    }
}
