using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phymnary.SugarPot.Module;

public interface IModule
{
    public void ConfigureServices(IServiceCollection services, IConfigurationManager configuration);
}
