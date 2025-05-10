using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest defining rules for sale creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleRequestValidator"/> class.
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number must be at most 50 characters.");

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
            .SetValidator(new CreateSaleItemRequestValidator());
    }
}
