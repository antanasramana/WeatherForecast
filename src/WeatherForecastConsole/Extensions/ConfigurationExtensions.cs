using Microsoft.Extensions.Configuration;

namespace WeatherForecastConsole.Extensions;

public static class ConfigurationExtensions
{
    public static void ValidateCitiesArgument(this IConfiguration configuration)
    {
        var cities = configuration["cities"];
        if (string.IsNullOrEmpty(cities))
        {
            throw new ArgumentNullException(nameof(cities),
                "Argument --cities was not provided or the list is empty");
        }
    }
}