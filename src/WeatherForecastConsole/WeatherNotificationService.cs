using Application.WeatherForecast;
using Domain;

namespace WeatherForecastConsole;

public class WeatherNotificationService : IWeatherNotificationService
{
    public void SendWeathers(IEnumerable<Weather> weathers)
    {
        foreach (var weather in weathers)
        {
            // Normally we would not use static Console here as we don't want this static dependency
            // We would create a wrapper class for it. But as this project is specifically designed for console interface - it's ok
            Console.WriteLine(weather.ToString());
        }
    }
}