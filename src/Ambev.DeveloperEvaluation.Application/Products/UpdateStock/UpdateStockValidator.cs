using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Validator for the update stock command
/// </summary>
public class UpdateStockValidator : AbstractValidator<UpdateStockCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateStockValidator
    /// </summary>
    public UpdateStockValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Valid product ID is required.");

        RuleFor(p => p.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
    }
}
