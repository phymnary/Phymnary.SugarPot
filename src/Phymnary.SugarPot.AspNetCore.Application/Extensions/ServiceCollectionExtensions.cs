using Microsoft.Extensions.DependencyInjection;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services.AddScoped<IRunAt, RunAt>();
    }
}
