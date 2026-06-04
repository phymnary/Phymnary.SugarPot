using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phymnary.SugarPot.Module.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSection<TConfig>(
        this IServiceCollection services,
        Action<TConfig>? setupWithConfig = null
    )
        where TConfig : class, IAppSettingsSection
    {
        var configurations = services.GetConfiguration();
        var section = configurations.GetSection(TConfig.Section);

        services.Configure<TConfig>(section.Bind);
        setupWithConfig?.Invoke(section.Get<TConfig>()!);

        return services;
    }

    public static IServiceCollection ConfigureSection<TConfig, TValidator>(
        this IServiceCollection services,
        Action<TConfig>? setupWithConfig = null
    )
        where TConfig : class, IAppSettingsSection
        where TValidator : class, IValidateOptions<TConfig>
    {
        services = ConfigureSection(services, setupWithConfig)
            .AddSingleton<IValidateOptions<TConfig>, TValidator>();

        return services;
    }

    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        return services.GetConfigurationOrNull()
            ?? throw new Exception(
                $"Could not find an implementation of {typeof(IConfiguration).AssemblyQualifiedName} in the service collection."
            );
    }

    private static IConfiguration? GetConfigurationOrNull(this IServiceCollection services)
    {
        var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
        return hostBuilderContext?.Configuration != null
            ? hostBuilderContext.Configuration
            : services.GetSingletonInstanceOrNull<IConfiguration>();
    }

    private static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        return (T?)
            services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.NormalizedImplementationInstance();
    }
}
