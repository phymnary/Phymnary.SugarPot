namespace Phymnary.SugarPot.AspNetCore.Security;

internal class HttpContextCurrentUser : ICurrentUser
{
    public Guid? Id { get; set; }
}
