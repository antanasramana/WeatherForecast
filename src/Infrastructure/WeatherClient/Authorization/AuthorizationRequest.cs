namespace Infrastructure.WeatherClient.Authorization;

public class AuthorizationRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}