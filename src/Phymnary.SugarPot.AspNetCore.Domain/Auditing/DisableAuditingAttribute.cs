namespace Phymnary.SugarPot.AspNetCore.Domain.Auditing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class DisableAuditingAttribute : Attribute;
