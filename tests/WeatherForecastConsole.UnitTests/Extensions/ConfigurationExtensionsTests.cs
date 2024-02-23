using FluentAssertions;
using Microsoft.Extensions.Configuration;
using WeatherForecastConsole.Extensions;

namespace WeatherForecastConsole.UnitTests.Extensions;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void ValidateCitiesArgument_ThrowsException_WhenCitiesNotProvided()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .Build();

        // Act & Assert
        config.Invoking(c => c.ValidateCitiesArgument())
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValidateCitiesArgument_NoException_WhenCitiesProvided()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("cities", "New York, London, Tokyo")
            })
            .Build();

        // Act & Assert
        config.Invoking(c => c.ValidateCitiesArgument()).Should().NotThrow();
    }
}