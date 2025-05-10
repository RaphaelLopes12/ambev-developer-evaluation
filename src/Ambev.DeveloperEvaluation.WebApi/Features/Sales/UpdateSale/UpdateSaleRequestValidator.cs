using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest defining rules for sale updates.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleRequestValidator"/> class.
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Sale date is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(100).WithMessage("Customer name must be at most 100 characters.");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name must be at most 100 characters.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one sale item is required.");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateSaleItemRequestValidator());
    }
}
