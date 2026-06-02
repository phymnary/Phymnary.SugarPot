using System.Collections.Immutable;

namespace Phymnary.SugarPot.AspNetCore.Domain.Auditings;

public class PropertyAuditing
{
    public bool IsAuditEnabled { get; set; }

    public required ImmutableHashSet<string> ValidAuditProperties { private get; init; }

    public required ImmutableHashSet<string> IgnoreAuditProperties { private get; init; }

    public bool IsValid(string name)
    {
        var isValid = ValidAuditProperties.IsEmpty || ValidAuditProperties.Contains(name);
        var isNotIgnored = !IgnoreAuditProperties.Contains(name);

        return isValid && isNotIgnored;
    }
}
