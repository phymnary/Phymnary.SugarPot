using System.Collections.Immutable;

namespace Phymnary.SugarPot.AspNetCore.Domain.Auditing;

internal class PropertyAuditing
{
    public bool IsAuditEnabled { get; set; }

    public required ImmutableArray<string> ValidAuditProperties { private get; init; }

    public required ImmutableArray<string> IgnoreAuditProperties { private get; init; }

    public bool IsValid(string name)
    {
        var isValid = ValidAuditProperties.Length == 0 || ValidAuditProperties.Contains(name);
        var isNotIgnored = !IgnoreAuditProperties.Contains(name);

        return isValid && isNotIgnored;
    }
}
