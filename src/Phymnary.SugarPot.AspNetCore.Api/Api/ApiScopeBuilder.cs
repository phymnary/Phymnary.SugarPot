using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.MultiTenancy;
using Phymnary.SugarPot.AspNetCore.Security;

namespace Phymnary.SugarPot.AspNetCore.Api;

internal class ApiScopeBuilder(IServiceScopeFactory factory) : IScopeBuilder
{
    public AsyncServiceScope Initialize(ScopeContext context)
    {
        var scope = factory.CreateAsyncScope();

        if (
            scope.ServiceProvider.GetService<ICurrentTenant>()
            is HttpContextCurrentTenant currentTenant
        )
        {
            currentTenant.Id = context.CurrentTenantId;
        }

        if (scope.ServiceProvider.GetService<ICurrentUser>() is HttpContextCurrentUser currentUser)
        {
            currentUser.Id = context.CurrentUserId;
        }

        if (
            scope.ServiceProvider.GetService<IAbortedToken>()
            is HttpContextAbortedProvider abortedProvider
        )
        {
            abortedProvider.Set(context.RequestAborted ?? default);
        }

        return scope;
    }
}
