using FluentValidation;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.ModID)
            .NotEmpty()
            .WithMessage("Model ID is required");

        RuleFor(p => p.ModName)
            .NotEmpty()
            .WithMessage("Model name is required")
            .MinimumLength(3)
            .WithMessage("Model name must be at least 3 characters");

        RuleFor(p => p.PriceWS)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}
