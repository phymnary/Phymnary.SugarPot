using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.Api.ExceptionHandler;
using Phymnary.SugarPot.AspNetCore.MultiTenancy;
using Phymnary.SugarPot.AspNetCore.Security;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Default Implementation for <see cref="ICurrentUser"/>, <see cref="ICurrentTenant"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddBoilerplateServices(this IServiceCollection services)
    {
        return services
            .AddScoped<RunAt>()
            .AddScoped<ICurrentUser, HttpContextCurrentUser>()
            .AddScoped<ICurrentTenant, HttpContextCurrentTenant>()
            .AddScoped<IAbortedToken, HttpContextAbortedProvider>()
            .AddProblemDetails()
            .AddExceptionHandler<AspExceptionHandler>();
    }
}
