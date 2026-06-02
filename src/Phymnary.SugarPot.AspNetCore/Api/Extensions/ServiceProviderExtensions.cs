using Microsoft.Extensions.DependencyInjection;

namespace Phymnary.SugarPot.AspNetCore.Api.Extensions;

public static class ServiceProviderExtensions
{
    public static AsyncServiceScope InheritAsyncServiceScope(this IServiceProvider provider)
    {
        var scope = provider.CreateAsyncScope();

        InheritValue<ICurrentTenant>(
            provider,
            scope.ServiceProvider,
            (higher, curr) =>
            {
                curr.Id = higher.Id;
            }
        );
        InheritValue<ICurrentUser>(
            provider,
            scope.ServiceProvider,
            (higher, curr) =>
            {
                curr.Id = higher.Id;
            }
        );
        InheritValue<IAbortedProvider>(
            provider,
            scope.ServiceProvider,
            (higher, curr) =>
            {
                curr.Set(higher.Get(default));
            }
        );

        return scope;
    }

    private static void InheritValue<T>(
        IServiceProvider higherProvider,
        IServiceProvider newProvider,
        Action<T, T> mapFn
    )
        where T : notnull
    {
        var higherScopedService = higherProvider.GetRequiredService<T>();
        var newService = newProvider.GetRequiredService<T>();
        mapFn(higherScopedService, newService);
    }
}
