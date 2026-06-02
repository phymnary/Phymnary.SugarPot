namespace Phymnary.SugarPot.AspNetCore.Domain.Security;

public interface ICurrentUser
{
    Guid? Id { get; set; }
}
