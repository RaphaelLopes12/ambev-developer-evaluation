using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleItemRequest defining rules for sale item updates.
/// </summary>
public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleItemRequestValidator"/> class.
    /// </summary>
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must be at most 100 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
    }
}
