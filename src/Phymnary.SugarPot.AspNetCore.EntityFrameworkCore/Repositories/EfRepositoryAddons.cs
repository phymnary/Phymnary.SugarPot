namespace Phymnary.SugarPot.AspNetCore.Repositories;

public class EfRepositoryAddons(EfDbStateManager dbStateManager, IAbortedToken abortedProvider)
{
    public IAbortedToken AbortedProvider => abortedProvider;

    public EfDbStateManager DbStateManager => dbStateManager;
}
