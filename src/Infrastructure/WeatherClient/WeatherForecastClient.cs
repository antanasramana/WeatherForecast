using System.Net.Http.Json;
using Application.WeatherForecast;
using Domain;

namespace Infrastructure.WeatherClient;

public class WeatherForecastClient : IWeatherForecastClient
{
    private readonly HttpClient _httpClient;

    public WeatherForecastClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<string>> GetCities() => 
        await _httpClient.GetFromJsonAsync<IList<string>>("cities");

    public async Task<Weather> GetWeather(string city) =>
        await _httpClient.GetFromJsonAsync<Weather>($"weathers/{city}");
}