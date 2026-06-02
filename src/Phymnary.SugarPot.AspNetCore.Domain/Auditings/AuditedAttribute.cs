namespace Phymnary.SugarPot.AspNetCore.Domain.Auditings;

/// <summary>
/// Specific what properties need to be audited of target class
/// </summary>
/// <param name="properties">Only audited these properties</param>
[AttributeUsage(AttributeTargets.Class)]
public class AuditedAttribute(params string[] properties) : Attribute
{
    public string[] Properties { get; } = properties;
}
