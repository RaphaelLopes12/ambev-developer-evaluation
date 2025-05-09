using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;

/// <summary>
/// Validator for products by category requests
/// </summary>
public class GetProductsByCategoryRequestValidator : AbstractValidator<GetProductsByCategoryRequest>
{
    public GetProductsByCategoryRequestValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Size cannot exceed 100");
    }
}
