using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Infrastructure.WeatherClient.Authorization;

public class AuthorizationDelegatingHandler
    : DelegatingHandler
{
    private readonly WeatherForecastClientOptions _options;

    public AuthorizationDelegatingHandler(IOptions<WeatherForecastClientOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorizationResponse = await GetAccessTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationResponse.Token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<AuthorizationResponse> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var authorizationRequest = new AuthorizationRequest
        { Username = _options.Username, Password = _options.Password };

        var serializedRequest = JsonSerializer.Serialize(authorizationRequest);
        var requestStringContent = new StringContent(serializedRequest, new MediaTypeHeaderValue("application/json"));

        var baseUri = new Uri(_options.ApiUrl);
        var authUri = new Uri(baseUri, "authorize");

        var authRequest = new HttpRequestMessage(
            HttpMethod.Post,
            authUri)
        {
            Content = requestStringContent
        };

        var response = await base.SendAsync(authRequest, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AuthorizationResponse>(cancellationToken);
    }
}