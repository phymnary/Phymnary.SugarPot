namespace Phymnary.SugarPot.AspNetCore.Api;

public class HttpContextCurrentUser : ICurrentUser
{
    public Guid? Id { get; set; }
}
