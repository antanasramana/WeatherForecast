using Application.WeatherForecast;
using Domain;
using Moq;

namespace Application.UnitTests;

public class WeatherForecastService_SaveWeathersTests
{
    private readonly Mock<IWeatherForecastClient> _weatherForecastClientMock = new();
    private readonly Mock<TimeProvider> _timeProviderMock = new();
    private readonly Mock<IRepository<Weather>> _weatherRepositoryMock = new();

    private readonly WeatherForecastService _weatherForecastService;

    public WeatherForecastService_SaveWeathersTests()
    {
        _weatherForecastService = new WeatherForecastService(
            _weatherForecastClientMock.Object,
            _timeProviderMock.Object,
            _weatherRepositoryMock.Object);
    }

    [Fact]
    public async Task SaveWeathers_ShouldSaveEachWeather()
    {
        // Arrange
        var weathers = new List<Weather>
        {
            new() { City = "City1", Precipitation = 89, WindSpeed = 15.3, Temperature = 18, Summary = "Cool" },
            new() { City = "City2", Precipitation = 100, WindSpeed = 2d, Temperature = 20, Summary = "Sunny" },
        };

        foreach (var weather in weathers)
        {
            _weatherRepositoryMock.Setup(x => x.Create(weather))
                .Returns(Task.CompletedTask);
        }

        // Act
        await _weatherForecastService.SaveWeathers(weathers);

        // Assert
        foreach (var weather in weathers)
        {
            _weatherRepositoryMock.Verify(x => x.Create(weather), Times.Once);
        }
    }
}