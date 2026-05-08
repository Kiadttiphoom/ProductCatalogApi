using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalogApi.Middleware;
using Xunit;
using FluentAssertions;

namespace TestApi.Tests.Middleware;

public class ApiKeyMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<ApiKeyMiddleware>> _mockLogger;
    private readonly ApiKeyMiddleware _middleware;
    private const string ValidApiKey = "TestApiKey123";

    public ApiKeyMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<ApiKeyMiddleware>>();

        // Setup mock configuration to return our valid key
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(s => s.Value).Returns(ValidApiKey);
        _mockConfiguration.Setup(c => c.GetSection("Authentication:ApiKey")).Returns(mockSection.Object);

        _middleware = new ApiKeyMiddleware(_mockNext.Object, _mockConfiguration.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task InvokeAsync_WithValidApiKey_ShouldCallNext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["X-API-KEY"] = ValidApiKey;
        context.Request.Path = "/api/products";

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task InvokeAsync_WithMissingApiKey_ShouldReturn401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/products";
        // Ensure the response body is writeable
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidApiKey_ShouldReturn401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["X-API-KEY"] = "WrongKey";
        context.Request.Path = "/api/products";
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/swagger/index.html")]
    [InlineData("/health")]
    public async Task InvokeAsync_WithExcludedPath_ShouldSkipCheck(string path)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = path;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        context.Response.StatusCode.Should().NotBe(StatusCodes.Status401Unauthorized);
    }
}
