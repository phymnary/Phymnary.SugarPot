using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.Repositories;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfCoreServices<TDbContext>(
        this IServiceCollection services,
        Action<EfServicesConfigurator<TDbContext>> configure
    )
        where TDbContext : DbContext
    {
        var configurator = new EfServicesConfigurator<TDbContext>(services);
        configure(configurator);

        return ConfigUnderlayAddEfServices(configurator);
    }

    public static IServiceCollection AddEfCoreServices<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var configurator = new EfServicesConfigurator<TDbContext>(services);

        return ConfigUnderlayAddEfServices(configurator);
    }

    private static IServiceCollection ConfigUnderlayAddEfServices<TDbContext>(
        EfServicesConfigurator<TDbContext> configurator
    )
        where TDbContext : DbContext
    {
        return configurator
            .Build()
            .AddScoped<IDbFunctionProvider, DbFunctionProvider<TDbContext>>()
            .AddScoped<EfRepositoryAddons>();
    }
}
