using Application;
using Application.WeatherForecast;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherForecastConsole;
using WeatherForecastConsole.Extensions;

var logger = LoggingSetup.InitializeGlobalLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args);
    
    var host = builder.ConfigureServices((context, services) =>
        {
            context.Configuration.ValidateCitiesArgument();

            services.AddInfrastructure(context.Configuration);
            services.AddApplication(context.Configuration);
            services.AddTransient<IWeatherNotificationService, WeatherNotificationService>();
            services.AddTransient<WeatherForecastConsoleApplication>();
            services.Configure<ConsoleOptions>(options =>
                options.Cities = context.Configuration["cities"]!);
        })
        .UseLogging()
        .Build();

    using var scope = host.Services.CreateScope();

    scope.CreateSqlLiteDatabase();
    var weatherForecastConsoleApplication = scope.ServiceProvider
        .GetRequiredService<WeatherForecastConsoleApplication>();

    await weatherForecastConsoleApplication.Start(host);
}
catch (Exception exception)
{
    logger.LogError(exception, "An exception has occurred");
}
finally
{
    LoggingSetup.CloseAndFlushGlobalLogger();
}
