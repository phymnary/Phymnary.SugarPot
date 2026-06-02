namespace Phymnary.SugarPot.AspNetCore.Repositories;

public class EfRepositoryAddons(IAbortedToken abortedProvider)
{
    public IAbortedToken AbortedProvider => abortedProvider;
}
