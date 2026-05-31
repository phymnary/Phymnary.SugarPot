using Microsoft.Extensions.DependencyInjection;

namespace Phymnary.SugarPot.DependencyInjection;

public interface IAutoRegister
{
    void AddDependencies(IServiceCollection services);
}
