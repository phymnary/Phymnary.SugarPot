namespace AspNetCore.Boilerplate.EntityFrameworkCore;

public interface IAbortedProvider
{
    CancellationToken Get(CancellationToken cancellationToken);

    void Set(CancellationToken cancellationToken);
}
