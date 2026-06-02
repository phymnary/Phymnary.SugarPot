namespace Phymnary.SugarPot.AspNetCore;

public interface IAbortedToken
{
    CancellationToken Get(CancellationToken cancellationToken);

    void Set(CancellationToken cancellationToken);
}
