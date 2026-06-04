using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.MultiTenancy;
using Phymnary.SugarPot.AspNetCore.Security;

namespace Phymnary.SugarPot.AspNetCore.Api.Extensions;

public static class ServiceScopeFactoryExtensions
{
    public static AsyncServiceScope InitAsyncScopeWithContext(
        this IServiceScopeFactory factory,
        HttpContextProvider contextProvider
    )
    {
        var scope = factory.CreateAsyncScope();
        if (
            contextProvider.CurrentTenantId is { } currentTenantId
            && scope.ServiceProvider.GetRequiredService<ICurrentTenant>()
                is HttpContextCurrentTenant currentTenant
        )
        {
            currentTenant.Id = currentTenantId;
        }

        if (
            contextProvider.CurrentUserId is { } currentUserId
            && scope.ServiceProvider.GetRequiredService<ICurrentUser>()
                is HttpContextCurrentUser currentUser
        )
        {
            currentUser.Id = currentUserId;
        }

        if (
            contextProvider.RequestAborted is { } requestAborted
            && scope.ServiceProvider.GetRequiredService<IAbortedToken>()
                is HttpContextAbortedProvider abortedProvider
        )
        {
            abortedProvider.Set(requestAborted);
        }

        return scope;
    }
}
