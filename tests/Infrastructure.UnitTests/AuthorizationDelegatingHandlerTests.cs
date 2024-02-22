using System.Net;
using System.Text.Json;
using FluentAssertions;
using Infrastructure.WeatherClient;
using Infrastructure.WeatherClient.Authorization;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Infrastructure.UnitTests;

public class AuthorizationDelegatingHandlerTests
{
    private readonly Mock<IOptions<WeatherForecastClientOptions>> _optionsMock = new();

    public AuthorizationDelegatingHandlerTests()
    {
        _optionsMock.Setup(x => x.Value).Returns(new WeatherForecastClientOptions
        {
            Username = "testUser",
            Password = "testPassword",
            ApiUrl = "https://example.com/"
        });
    }

    [Fact]
    public async Task SendAsync_WhenStatusCodeOk_ShouldSetAuthorizationHeaderWithAccessToken()
    {
        // Arrange
        const string accessToken = "testAccessToken";
        var authorizationResponse = new AuthorizationResponse { Token = accessToken };

        var httpHandlerMock = new Mock<HttpMessageHandler>();
        httpHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(authorizationResponse))
            });

        var authorizationDelegatingHandler = new AuthorizationDelegatingHandler(_optionsMock.Object)
        {
            InnerHandler = httpHandlerMock.Object
        };

        var invoker = new HttpMessageInvoker(authorizationDelegatingHandler);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/endpoint");

        // Act
        await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        request.Headers.Authorization.Should().NotBeNull();
        request.Headers.Authorization.Scheme.Should().Be("Bearer");
        request.Headers.Authorization.Parameter.Should().Be(accessToken);
    }

    [Fact]
    public async Task SendAsync_WhenStatusCodeUnauthorized_ShouldThrowHttpRequestException()
    {
        //Arrange
        var httpHandlerMock = new Mock<HttpMessageHandler>();
        httpHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        var authorizationDelegatingHandler = new AuthorizationDelegatingHandler(_optionsMock.Object)
        {
            InnerHandler = httpHandlerMock.Object
        };

        var invoker = new HttpMessageInvoker(authorizationDelegatingHandler);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/endpoint");

        // Act
        Func<Task> act = async () => await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}