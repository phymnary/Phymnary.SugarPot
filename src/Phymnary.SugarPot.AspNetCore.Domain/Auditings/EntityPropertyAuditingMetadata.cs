using System.Collections.Frozen;

namespace Phymnary.SugarPot.AspNetCore.Auditings;

public class EntityPropertyAuditingMetadata
{
    public bool IsAuditEnabled { get; set; }

    public required FrozenSet<string> ValidAuditProperties { private get; init; }

    public required FrozenSet<string> IgnoreAuditProperties { private get; init; }

    public bool CanAudit(string name)
    {
        var isValid = ValidAuditProperties.Count == 0 || ValidAuditProperties.Contains(name);
        var isNotIgnored = !IgnoreAuditProperties.Contains(name);

        return isValid && isNotIgnored;
    }
}
