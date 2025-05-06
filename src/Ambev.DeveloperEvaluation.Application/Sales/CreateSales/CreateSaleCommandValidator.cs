using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    /// <summary>
    /// Validator for <see cref="CreateSaleCommand"/>.
    /// Ensures that all required fields are valid and consistent with business rules.
    /// </summary>
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(c => c.Number)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

            RuleFor(c => c.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(c => c.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(c => c.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(c => c.BranchName)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

            RuleForEach(c => c.Items)
                .SetValidator(new CreateSaleItemDtoValidator());
        }
    }
}
