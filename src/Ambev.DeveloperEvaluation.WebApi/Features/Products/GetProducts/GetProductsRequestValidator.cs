using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Validator for product list requests
/// </summary>
public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Size cannot exceed 100");
    }
}
