namespace Phymnary.SugarPot.AspNetCore.MultiTenancy;

public class HttpContextCurrentTenant : ICurrentTenant
{
    public Guid? Id { get; set; }
}
