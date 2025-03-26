using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RPSSLGame.Application;
using RPSSLGame.Infrastructure;

namespace RPSSLGame.DI;

public static class IServiceCollectionExtensions
{
    public static void Register(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterInfrastructureServices(configuration);
        services.RegisterApplicationServices();
    }
}
