using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RPSSLGame.Application;
using RPSSLGame.Infrastructure;

namespace RPSSLGame.DI;

public static class IServiceCollectionExtensions
{
    public static void Register(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services
        services.RegisterInfrastructureServices(configuration);

        // Register application services
        services.RegisterApplicationServices();
    }
}
