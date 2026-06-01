using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phymnary.SugarPot.AspNetCore;
using Phymnary.SugarPot.AspNetCore.Api;
using Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;
using Phymnary.SugarPot.DependencyInjection;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

/// <summary>
/// UseDeveloperExceptionPage is added first when the HostingEnvironment is "Development".<br/>
/// UseRouting is added second if user code didn't already call UseRouting and if there are endpoints configured, for example app.MapGet.<br/>
/// UseEndpoints is added at the end of the middleware pipeline if any endpoints are configured.<br/>
/// UseAuthentication is added immediately after UseRouting if user code didn't already call UseAuthentication and if IAuthenticationSchemeProvider can be detected in the service provider. IAuthenticationSchemeProvider is added by default when using AddAuthentication, and services are detected using IServiceProviderIsService.<br/>
/// UseAuthorization is added next if user code didn't already call UseAuthorization and if IAuthorizationHandlerProvider can be detected in the service provider. IAuthorizationHandlerProvider is added by default when using AddAuthorization, and services are detected using IServiceProviderIsService.<br/>
/// User configured middleware and endpoints are added between UseRouting and UseEndpoints.
/// </summary>
public static class ApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddModule<TModule>(this IHostApplicationBuilder builder)
        where TModule : IModule, new()
    {
        var module = new TModule();
        var services = builder.Services;
        module.ConfigureServices(services, builder.Configuration);

        if (module is IAutoRegister autoRegister)
            autoRegister.AddDependencies(services);

        return builder;
    }

    public static WebApplication UseBoilerplateServices(this WebApplication app)
    {
        app.Use(
            (context, next) =>
            {
                if (
                    context.RequestServices.GetService<IAbortedProvider>()
                    is HttpContextAbortedProvider provider
                )
                    provider.Set(context.RequestAborted);

                if (
                    context.User.FindFirstValue("sub") is { } subId
                    && Guid.TryParse(subId, out var currentUserId)
                    && context.RequestServices.GetService<ICurrentUser>()
                        is HttpContextCurrentUser currentUser
                )
                    currentUser.Id = currentUserId;

                if (
                    EfFeatureFlags.IsMultiTenantEnable
                    && context.User.FindFirstValue("tenant") is { } tenantId
                    && Guid.TryParse(tenantId, out var currentTenantId)
                    && context.RequestServices.GetService<ICurrentTenant>()
                        is HttpContextCurrentTenant currentTenant
                )
                    currentTenant.Id = currentTenantId;

                return next(context);
            }
        );

        app.UseExceptionHandler();

        return app;
    }
}
