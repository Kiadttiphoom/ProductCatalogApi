using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogApi.Validators;

namespace ProductCatalogApi.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidationConfiguration(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ProductValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
