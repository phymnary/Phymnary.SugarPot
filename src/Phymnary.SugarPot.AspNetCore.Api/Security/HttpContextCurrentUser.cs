namespace Phymnary.SugarPot.AspNetCore.Security;

public class HttpContextCurrentUser : ICurrentUser
{
    public Guid? Id { get; set; }
}
