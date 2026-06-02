namespace Phymnary.SugarPot.AspNetCore.Security;

public interface ICurrentUser
{
    Guid? Id { get; set; }
}
