namespace Phymnary.SugarPot.AspNetCore.Api;

public class HttpContextCurrentTenant : ICurrentTenant
{
    public Guid? Id { get; set; }
}
