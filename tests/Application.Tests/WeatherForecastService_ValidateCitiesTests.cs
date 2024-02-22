using Application.WeatherForecast;
using Domain;
using FluentAssertions;
using Moq;

namespace Application.UnitTests;

public class WeatherForecastService_ValidateCitiesTests
{
    private readonly Mock<IWeatherForecastClient> _weatherForecastClientMock = new();
    private readonly Mock<TimeProvider> _timeProviderMock = new();
    private readonly Mock<IRepository<Weather>> _weatherRepositoryMock = new();

    private readonly WeatherForecastService _weatherForecastService;

    public WeatherForecastService_ValidateCitiesTests()
    {
        _weatherForecastService = new WeatherForecastService(
            _weatherForecastClientMock.Object,
            _timeProviderMock.Object,
            _weatherRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidateCities_WhenCityIsNotAvailable_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var cities = new List<string> { "City1", "City2", "City3" };
        var availableCities = new List<string> { "City1", "City3", "City4", "City5" };

        _weatherForecastClientMock.Setup(x => x.GetCities())
                                 .ReturnsAsync(availableCities);

        // Act & Assert
        var action = async () => await _weatherForecastService.ValidateCities(cities);
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task ValidateCities_WhenAllCitiesAvailable_ShouldNotThrowException()
    {
        // Arrange
        var cities = new List<string> { "City1", "City2", "City3" };
        var availableCities = new List<string> { "City1", "City2", "City3", "City4", "City5" };

        _weatherForecastClientMock.Setup(x => x.GetCities())
                                 .ReturnsAsync(availableCities);

        // Act & Assert
        var action = async () => await _weatherForecastService.ValidateCities(cities);
        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task ValidateCities_WhenNoCitiesProvided_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var cities = new List<string>();

        // Act & Assert
        var action = async () => await _weatherForecastService.ValidateCities(cities);
        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>();
    }
}