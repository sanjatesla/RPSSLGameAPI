using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PSSLGame.Domain.Repositories;
using PSSLGame.Domain.Services;
using RPSSLGame.Infrastructure.Logging;
using RPSSLGame.Infrastructure.Repositories;
using RPSSLGame.Infrastructure.Services;
using Serilog;
using Serilog.Extensions.Logging;
using System.Net.Http.Json;

namespace RPSSLGame.Infrastructure;

public static class IServiceCollectionExtension
{
    private static string envKey = "ASPNETCORE_ENVIRONMENT";
    private static string devEnv = "Development";

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Serilog
        services.AddSerilog(configuration);

        // Register services
        services.AddHttpClient<IChoiceGenerator, ChoiceGenerator>();
        services.AddTransient<IScoreboardRepository, ScoreboardRepository>();
        services.AddSingleton<ScoreboardInMemory>();

        return services;
    }

    private static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        var environmentVariables = Environment.GetEnvironmentVariables();

        var environment = environmentVariables.Contains(envKey) ?
            environmentVariables[envKey].ToString() : devEnv;

        Log.Logger = SerilogConfig.ConfigureLogging(configuration, environment);

        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

        services.AddSingleton<ILoggerFactory>(s => new SerilogLoggerFactory(Log.Logger, false));

        return services;
    }
}
