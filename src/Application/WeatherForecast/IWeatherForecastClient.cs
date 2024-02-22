using Domain;

namespace Application.WeatherForecast;

public interface IWeatherForecastClient
{
    Task<IList<string>> GetCities();
    Task<Weather> GetWeather(string city);
}