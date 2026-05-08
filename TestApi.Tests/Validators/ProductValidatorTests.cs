using FluentAssertions;
using FluentValidation;
using ProductCatalogApi.Models.Entities;
using ProductCatalogApi.Validators;
using Xunit;

namespace TestApi.Tests.Validators;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    [Fact]
    public void Validate_WithValidProduct_ShouldPass()
    {
        // Arrange
        var product = new Product { ModID = "1", ModName = "Valid Product", PriceWS = 100 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var product = new Product { ModID = "1", ModName = "", PriceWS = 100 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ModName");
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldFail()
    {
        // Arrange
        var product = new Product { ModID = "1", ModName = "Product", PriceWS = -10 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PriceWS");
    }

    [Fact]
    public void Validate_WithShortName_ShouldFail()
    {
        // Arrange
        var product = new Product { ModID = "1", ModName = "AB", PriceWS = 100 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
