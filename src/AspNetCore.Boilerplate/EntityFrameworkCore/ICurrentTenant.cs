namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

public interface ICurrentTenant
{
    Guid? Id { get; set; }
}
