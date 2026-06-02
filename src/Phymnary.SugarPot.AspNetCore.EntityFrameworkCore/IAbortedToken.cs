namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

public interface IAbortedToken
{
    CancellationToken Get(CancellationToken cancellationToken);

    void Set(CancellationToken cancellationToken);
}
