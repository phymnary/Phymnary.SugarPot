using Microsoft.Extensions.Hosting;
using Phymnary.SugarPot.DependencyInjection;

namespace Phymnary.SugarPot.Module.Extensions;

/// <summary>
/// UseDeveloperExceptionPage is added first when the HostingEnvironment is "Development".<br/>
/// UseRouting is added second if user code didn't already call UseRouting and if there are endpoints configured, for example app.MapGet.<br/>
/// UseEndpoints is added at the end of the middleware pipeline if any endpoints are configured.<br/>
/// UseAuthentication is added immediately after UseRouting if user code didn't already call UseAuthentication and if IAuthenticationSchemeProvider can be detected in the service provider. IAuthenticationSchemeProvider is added by default when using AddAuthentication, and services are detected using IServiceProviderIsService.<br/>
/// UseAuthorization is added next if user code didn't already call UseAuthorization and if IAuthorizationHandlerProvider can be detected in the service provider. IAuthorizationHandlerProvider is added by default when using AddAuthorization, and services are detected using IServiceProviderIsService.<br/>
/// User configured middleware and endpoints are added between UseRouting and UseEndpoints.
/// </summary>
public static class HostApplicationBuilderExtensions
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
}
