namespace AspNetCore.Boilerplate.EntityFrameworkCore.Repositories;

public class EfRepositoryAddons(IAbortedProvider abortedProvider)
{
    public IAbortedProvider AbortedProvider => abortedProvider;
}
