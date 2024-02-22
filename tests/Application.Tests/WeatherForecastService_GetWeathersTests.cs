using Application.WeatherForecast;
using Domain;
using FluentAssertions;
using Moq;

namespace Application.UnitTests;

public class WeatherForecastService_GetWeathersTests
{
    private readonly Mock<IWeatherForecastClient> _weatherForecastClientMock = new();
    private readonly Mock<TimeProvider> _timeProviderMock = new();
    private readonly Mock<IRepository<Weather>> _weatherRepositoryMock = new();

    private readonly WeatherForecastService _weatherForecastService;

    public WeatherForecastService_GetWeathersTests()
    {
        _weatherForecastService = new WeatherForecastService(
            _weatherForecastClientMock.Object,
            _timeProviderMock.Object,
            _weatherRepositoryMock.Object);
    }

    [Fact]
    public async Task GetWeathers_ShouldReturnWeatherForEachCity()
    {
        var now = DateTimeOffset.Now;
        _timeProviderMock
            .Setup(x => x.GetUtcNow())
            .Returns(now);

        // Arrange
        var cities = new List<string> { "City1", "City2" };
        var expectedWeathers = new List<Weather>
        {
            new() { City = "City1", Precipitation = 89, WindSpeed = 15.3, Temperature = 18, Summary = "Cool" },
            new() { City = "City2", Precipitation = 100, WindSpeed = 2d, Temperature = 20, Summary = "Sunny" },
        };

        foreach (var weather in expectedWeathers)
        {
            _weatherForecastClientMock.Setup(x => x.GetWeather(weather.City))
                .ReturnsAsync(weather);
        }

        // Act
        var actualWeathers = await _weatherForecastService.GetWeathers(cities);

        // Assert
        actualWeathers.Should()
            .BeEquivalentTo(expectedWeathers, o => o.Excluding(w => w.TimeStamp));
        actualWeathers.Should()
            .AllSatisfy(w => w.TimeStamp.Should().Be(now));
    }

    [Fact]
    public async Task GetWeathers_ShouldHandleEmptyCitiesList()
    {
        // Arrange
        var cities = new List<string>();

        // Act
        var actualWeathers = await _weatherForecastService.GetWeathers(cities);

        // Assert
        actualWeathers.Should().BeEmpty();
    }
}