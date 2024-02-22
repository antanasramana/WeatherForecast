using Application;
using Application.WeatherForecast;
using Domain;
using Infrastructure.WeatherClient;
using Infrastructure.WeatherClient.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure;

public static class Bootstrap
{
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<AuthorizationDelegatingHandler>();
        services.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(c =>
            c.BaseAddress = new Uri(configuration["WeatherForecastClientOptions:ApiUrl"] ??
                                    throw new InvalidOperationException("Cannot find WeatherForecastClientOptions:ApiUrl key")))
            .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

        services.AddDbContext<WeatherContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("WeatherDbContext")));

        services.Configure<WeatherForecastClientOptions>(
            configuration.GetSection(WeatherForecastClientOptions.Section));

        services.AddTransient<IRepository<Weather>, WeatherContextRepository<Weather>>();
    }

    // Usually we don't see the creation of database happening during runtime, but as we are creating standalone application
    // We have to make sure that upon starting the app - the database should be created
    public static void CreateSqlLiteDatabase(this IServiceScope scope)
    {
        var weatherContext = scope.ServiceProvider.GetRequiredService<WeatherContext>();
        weatherContext.Database.EnsureCreated();
    }
}