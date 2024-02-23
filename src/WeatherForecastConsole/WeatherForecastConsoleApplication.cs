using Application.WeatherForecast;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace WeatherForecastConsole;

public class WeatherForecastConsoleApplication
{
    private readonly IPeriodicalWeatherForecastService _periodicalWeatherForecastService;
    private readonly IEnumerable<string> _cities;

    public WeatherForecastConsoleApplication(
        IPeriodicalWeatherForecastService periodicalWeatherForecastService, 
        IOptions<ConsoleOptions> options)
    {
        _periodicalWeatherForecastService = periodicalWeatherForecastService;
        _cities = options.Value.Cities 
            .Split(',', StringSplitOptions.TrimEntries 
                        | StringSplitOptions.RemoveEmptyEntries);
    }

    public async Task Start(IHost host)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        Console.WriteLine("Press CTRL + C to terminate the task");

        var getAndSaveTask = _periodicalWeatherForecastService
            .PeriodicallyGetAndSaveCitiesWeather(_cities, cancellationTokenSource.Token);

        var runHostTask = host.RunAsync();

        await Task.WhenAny(getAndSaveTask, runHostTask);
        await cancellationTokenSource.CancelAsync();
    }
}