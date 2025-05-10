using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// Validator for stock update requests
/// </summary>
public class UpdateStockRequestValidator : AbstractValidator<UpdateStockRequest>
{
    public UpdateStockRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");
    }
}
