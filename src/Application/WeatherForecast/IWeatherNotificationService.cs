using Domain;

namespace Application.WeatherForecast;

public interface IWeatherNotificationService
{
    public void SendWeathers(IEnumerable<Weather> weathers);
}