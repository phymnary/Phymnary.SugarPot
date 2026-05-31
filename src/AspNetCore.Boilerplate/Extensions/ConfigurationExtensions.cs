using Microsoft.Extensions.Configuration;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder, string env)
    {
        return builder
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{env}.json", true)
            .AddJsonFile($"appsettings.{env}.user.json", true)
            .AddEnvironmentVariables();
    }
}
