using Domain;

namespace Application.WeatherForecast;

public interface IWeatherForecastService
{
    Task<IList<Weather>> GetWeathers(IEnumerable<string> cities);
    Task SaveWeathers(IEnumerable<Weather> weathers);
    Task ValidateCities(IEnumerable<string> cities);
}

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastClient _weatherForecastClient;
    private readonly TimeProvider _timeProvider;
    private readonly IRepository<Weather> _weatherRepository;

    public WeatherForecastService(IWeatherForecastClient weatherForecastClient,
        TimeProvider timeProvider,
        IRepository<Weather> weatherRepository)
    {
        _weatherForecastClient = weatherForecastClient;
        _timeProvider = timeProvider;
        _weatherRepository = weatherRepository;
    }

    public async Task<IList<Weather>> GetWeathers(IEnumerable<string> cities)
    {
        var weathers = new List<Weather>();

        foreach (var city in cities)
        {
            var weather = await _weatherForecastClient.GetWeather(city);
            weather.TimeStamp = _timeProvider.GetUtcNow();
            weathers.Add(weather);
        }

        return weathers;
    }

    public async Task SaveWeathers(IEnumerable<Weather> weathers)
    {
        foreach (var weather in weathers)
        {
            await _weatherRepository.Create(weather);
        }
    }

    public async Task ValidateCities(IEnumerable<string> cities)
    {
        var availableCities = await _weatherForecastClient.GetCities();
        var areCitiesValid = cities.All(city => availableCities.Contains(city));
        if (!areCitiesValid)
        {
            throw new ArgumentOutOfRangeException(nameof(cities), 
                "One or more provided cities are not available");
        }
    }
}