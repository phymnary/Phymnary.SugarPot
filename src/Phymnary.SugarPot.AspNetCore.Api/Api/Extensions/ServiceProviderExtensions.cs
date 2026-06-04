using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.MultiTenancy;
using Phymnary.SugarPot.AspNetCore.Security;

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
                ((HttpContextCurrentTenant)curr).Id = higher.Id;
            }
        );
        InheritValue<ICurrentUser>(
            provider,
            scope.ServiceProvider,
            (higher, curr) =>
            {
                ((HttpContextCurrentUser)curr).Id = higher.Id;
            }
        );
        InheritValue<IAbortedToken>(
            provider,
            scope.ServiceProvider,
            (higher, curr) =>
            {
                ((HttpContextAbortedProvider)curr).Set(higher.Get(default));
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
