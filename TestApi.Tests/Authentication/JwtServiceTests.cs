using FluentAssertions;
using ProductCatalogApi.Authentication;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi.Tests.Authentication;

public class JwtServiceTests
{
    private readonly JwtService _service;
    private readonly Mock<ILogger<JwtService>> _mockLogger;

    public JwtServiceTests()
    {
        var settings = new JwtSettings
        {
            Secret = "this-is-a-very-long-secret-key-for-testing-jwt-tokens-minimum-32-chars",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationMinutes = 60
        };

        _mockLogger = new Mock<ILogger<JwtService>>();
        _service = new JwtService(settings, _mockLogger.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Act
        var token = _service.GenerateToken("user123", "testuser");

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateToken_WithValidParameters_ShouldReturnJwtToken()
    {
        // Act
        var token = _service.GenerateToken("user123", "testuser");

        // Assert
        token.Should().Contain(".");
        var parts = token.Split('.');
        parts.Should().HaveCount(3);
    }

    [Fact]
    public void ValidateToken_WithValidToken_ShouldReturnTrue()
    {
        // Arrange
        var token = _service.GenerateToken("user123", "testuser");

        // Act
        var isValid = _service.ValidateToken(token);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateToken_WithInvalidToken_ShouldReturnFalse()
    {
        // Act
        var isValid = _service.ValidateToken("invalid.token.here");

        // Assert
        isValid.Should().BeFalse();
    }
}
