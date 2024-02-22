using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.WeatherForecast;

public interface IPeriodicalWeatherForecastService
{
    Task PeriodicallyGetAndSaveCitiesWeather(
        IEnumerable<string> cities,
        CancellationToken cancellationToken);
}

public class PeriodicalWeatherForecastService : IPeriodicalWeatherForecastService
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IWeatherNotificationService _weatherNotificationService;
    private readonly ILogger<PeriodicalWeatherForecastService> _logger;
    private readonly PeriodicalWeatherForecastServiceOptions _options;

    public PeriodicalWeatherForecastService(
        IWeatherForecastService weatherForecastService,
        IWeatherNotificationService weatherNotificationService,
        ILogger<PeriodicalWeatherForecastService> logger,
        IOptions<PeriodicalWeatherForecastServiceOptions> options)
    {
        _weatherForecastService = weatherForecastService;
        _weatherNotificationService = weatherNotificationService;
        _logger = logger;
        _options = options.Value;
    }

    public async Task PeriodicallyGetAndSaveCitiesWeather(
        IEnumerable<string> cities,
        CancellationToken cancellationToken)
    {
        await _weatherForecastService.ValidateCities(cities);

        var periodicTimer = new PeriodicTimer(_options.WeatherFetchingPeriod);
        try
        {
            while (await periodicTimer.WaitForNextTickAsync(cancellationToken))
            {
                var weathers = await _weatherForecastService.GetWeathers(cities);
                await _weatherForecastService.SaveWeathers(weathers);
                _weatherNotificationService.SendWeathers(weathers);
            }
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogInformation(exception, "Periodic timer was cancelled");
        }
    }
}