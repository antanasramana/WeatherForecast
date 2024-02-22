using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

namespace Infrastructure;

public static class LoggingSetup
{
    public static Microsoft.Extensions.Logging.ILogger InitializeGlobalLogger()
    {
        // Used in logging as default path
        Environment.CurrentDirectory = AppContext.BaseDirectory;
        const string appSettingsFileName = "appsettings.json";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appSettingsFileName)
            .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        Log.Logger = logger;

        const string categoryName = "Host";
        return new SerilogLoggerFactory(logger).CreateLogger(categoryName);
    }
    public static void CloseAndFlushGlobalLogger()
    {
        Log.CloseAndFlush();
    }

    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
        return hostBuilder;
    }
}