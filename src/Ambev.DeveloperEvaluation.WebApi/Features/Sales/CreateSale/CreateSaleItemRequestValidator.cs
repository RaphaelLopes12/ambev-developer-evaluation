using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleItemRequest defining rules for sale item creation.
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleItemRequestValidator"/> class.
    /// </summary>
    public CreateSaleItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must be at most 100 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}
