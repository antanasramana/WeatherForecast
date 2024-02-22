using Application.WeatherForecast;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Bootstrap
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IWeatherForecastService, WeatherForecastService>();
        services.AddTransient<IPeriodicalWeatherForecastService, PeriodicalWeatherForecastService>();
        services.Configure<PeriodicalWeatherForecastServiceOptions>(
            configuration.GetSection(PeriodicalWeatherForecastServiceOptions.Section));
    }
}