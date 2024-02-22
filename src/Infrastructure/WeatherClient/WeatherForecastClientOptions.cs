namespace Infrastructure.WeatherClient;

public class WeatherForecastClientOptions
{
    public const string Section = nameof(WeatherForecastClientOptions);
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string ApiUrl { get; set; }
}