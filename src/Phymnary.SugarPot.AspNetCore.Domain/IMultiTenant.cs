namespace Phymnary.SugarPot.AspNetCore.Domain;

public interface IMultiTenant
{
    Guid TenantId { get; set; }
}
