using FluentValidation;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Validators;

public class ProvinceValidator : AbstractValidator<Province>
{
    public ProvinceValidator()
    {
        RuleFor(p => p.PrvID)
            .NotEmpty()
            .WithMessage("Province ID is required");

        RuleFor(p => p.PrvName)
            .NotEmpty()
            .WithMessage("Province name is required")
            .MinimumLength(2)
            .WithMessage("Province name must be at least 2 characters");
    }
}
