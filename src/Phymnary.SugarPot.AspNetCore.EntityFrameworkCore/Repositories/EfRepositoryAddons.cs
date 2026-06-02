namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Repositories;

public class EfRepositoryAddons(IAbortedToken abortedProvider)
{
    public IAbortedToken AbortedProvider => abortedProvider;
}
