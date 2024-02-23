using Domain;
using FluentAssertions;

namespace WeatherForecastConsole.UnitTests;

public class WeatherNotificationServiceTests
{
    [Fact]
    public void SendWeathers_ShouldWriteWeatherInfoToConsole()
    {
        // Arrange
        var service = new WeatherNotificationService();
        var weathers = new List<Weather>
        {
            new() { Id = 1, Temperature = 20, Precipitation = 0, WindSpeed = 10.5, Summary = "Sunny", City = "New York" },
            new() { Id = 2, Temperature = 15, Precipitation = 5, WindSpeed = 7.8, Summary = "Cloudy", City = "London" }
        };

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        service.SendWeathers(weathers);

        // Assert
        var expectedOutput = $"{weathers[0]}{Environment.NewLine}" +
                             $"{weathers[1]}{Environment.NewLine}";

        consoleOutput.ToString().Should().Be(expectedOutput);
    }
}