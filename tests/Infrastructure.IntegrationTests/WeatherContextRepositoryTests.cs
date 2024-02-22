using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests;

public class WeatherContextRepositoryIntegrationTests
{
    [Fact]
    public async Task Create_ShouldAddEntityToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<WeatherContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using (var context = new WeatherContext(options))
        {
            var repository = new WeatherContextRepository<Weather>(context);
            var weather = new Weather
            {
                City = "TestCity",
                Temperature = 25,
                Precipitation = 5,
                WindSpeed = 10.5,
                Summary = "Sunny"
            };

            // Act
            await repository.Create(weather);
        }

        // Assert
        await using (var context = new WeatherContext(options))
        {
            var savedWeather = await context.Weathers.FirstOrDefaultAsync();

            savedWeather.Should().NotBeNull();
            savedWeather.City.Should().Be("TestCity");
            savedWeather.Temperature.Should().Be(25);
            savedWeather.Precipitation.Should().Be(5);
            savedWeather.WindSpeed.Should().Be(10.5);
            savedWeather.Summary.Should().Be("Sunny");
        }
    }
}