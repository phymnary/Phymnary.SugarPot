namespace Phymnary.SugarPot.AspNetCore;

public interface IAbortedToken
{
    CancellationToken Get(CancellationToken cancellationToken);
}
