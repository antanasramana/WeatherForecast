namespace Application.WeatherForecast;

public class PeriodicalWeatherForecastServiceOptions
{
    public const string Section = nameof(PeriodicalWeatherForecastServiceOptions);
    public TimeSpan WeatherFetchingPeriod { get; set; }
}