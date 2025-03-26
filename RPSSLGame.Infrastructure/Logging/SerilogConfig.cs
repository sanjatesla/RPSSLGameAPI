using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace RPSSLGame.Infrastructure.Logging;

public static class SerilogConfig
{
    private static string envPropertyName = "Environment";
    private static string logFolderPath = "\\logs\\{0}\\{1}\\log.txt";
    private static string _envVar = "ASPNETCORE_ENVIRONMENT";

    public static ILogger ConfigureLogging(IConfiguration configuration, string environment)
    {
        var appName = AppDomain.CurrentDomain.FriendlyName.Split('.').FirstOrDefault() ?? "App";
        var env = Environment.GetEnvironmentVariable(_envVar) ?? "N/A";

        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("appName", appName)
            .Enrich.WithProperty("env", env)
            .WriteTo.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {appName} {env}] {Message:lj}{NewLine}{Exception}");

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + string.Format(logFolderPath, appName, env);

        config = config.WriteTo.File(path, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10);

        config = config.Enrich.WithProperty(envPropertyName, environment)
            .ReadFrom.Configuration(configuration);

        Log.Logger = config.CreateLogger();

        return Log.Logger;
    }
}
