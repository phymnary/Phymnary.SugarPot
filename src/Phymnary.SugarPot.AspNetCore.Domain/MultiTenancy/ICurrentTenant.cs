namespace Phymnary.SugarPot.AspNetCore.Domain.MultiTenancy;

public interface ICurrentTenant
{
    Guid? Id { get; set; }
}
