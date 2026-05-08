using FluentValidation;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Validators;

public class DistrictValidator : AbstractValidator<District>
{
    public DistrictValidator()
    {
        RuleFor(d => d.AmpID)
            .NotEmpty()
            .WithMessage("District ID is required");

        RuleFor(d => d.AmpName)
            .NotEmpty()
            .WithMessage("District name is required")
            .MinimumLength(2)
            .WithMessage("District name must be at least 2 characters");

        RuleFor(d => d.PrvID)
            .NotEmpty()
            .WithMessage("Province ID is required");
    }
}
