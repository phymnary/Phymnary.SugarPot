using Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

namespace Phymnary.SugarPot.AspNetCore.Api;

public class HttpContextCurrentTenant : ICurrentTenant
{
    public Guid? Id { get; set; }
}
