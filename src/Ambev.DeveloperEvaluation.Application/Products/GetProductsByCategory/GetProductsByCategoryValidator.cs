using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Validator for the get products by category query
/// </summary>
public class GetProductsByCategoryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    /// <summary>
    /// Initializes a new instance of the GetProductsByCategoryValidator
    /// </summary>
    public GetProductsByCategoryValidator()
    {
        RuleFor(q => q.Category)
            .NotEmpty().WithMessage("Category name is required.");

        RuleFor(q => q.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than zero.");

        RuleFor(q => q.Size)
            .GreaterThan(0).WithMessage("Page size must be greater than zero.")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
    }
}
