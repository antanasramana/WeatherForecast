using Application.WeatherForecast;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.UnitTests;

public class PeriodicalWeatherForecastServiceTests
{
    private readonly Mock<IWeatherForecastService> _weatherForecastServiceMock = new();
    private readonly Mock<IWeatherNotificationService> _weatherNotificationServiceMock = new();
    private readonly Mock<ILogger<PeriodicalWeatherForecastService>> _loggerMock = new();
    private readonly Mock<IOptions<PeriodicalWeatherForecastServiceOptions>> _optionsMock = new();

    [Fact]
    public async Task PeriodicallyGetAndSaveCitiesWeather_ShouldInvokeDependenciesCorrectly()
    {
        // Arrange
        var cities = new List<string> { "City1", "City2" };
        using var cancellationTokenSource = new CancellationTokenSource();

        var options = new PeriodicalWeatherForecastServiceOptions 
            { WeatherFetchingPeriod = TimeSpan.FromSeconds(1) };
        _optionsMock.SetupGet(x => x.Value)
            .Returns(options);

        var periodicalWeatherForecastService = new PeriodicalWeatherForecastService(
            _weatherForecastServiceMock.Object,
            _weatherNotificationServiceMock.Object,
            _loggerMock.Object,
            _optionsMock.Object);

        // Act
        var getAndSaveCitiesWeatherTask = periodicalWeatherForecastService
            .PeriodicallyGetAndSaveCitiesWeather(cities, cancellationTokenSource.Token);

        // Simulate a delay to make sure that the period was hit
        const int millisecondsToWait = 2 * 1000;
        await Task.Delay(millisecondsToWait);

        // Assert
        _weatherForecastServiceMock.Verify(x => x.ValidateCities(cities), Times.Once);
        _weatherForecastServiceMock.Verify(x => x.GetWeathers(cities), Times.AtLeastOnce);
        _weatherForecastServiceMock.Verify(x => x.SaveWeathers(It.IsAny<IEnumerable<Weather>>()), Times.AtLeastOnce);
        _weatherNotificationServiceMock.Verify(x => x.SendWeathers(It.IsAny<IEnumerable<Weather>>()), Times.AtLeastOnce);
        
        // Cleanup
        await cancellationTokenSource.CancelAsync();
        await getAndSaveCitiesWeatherTask;
    }
}