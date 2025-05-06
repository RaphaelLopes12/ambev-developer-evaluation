using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(s => s.Number)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(20).WithMessage("Sale number cannot exceed 20 characters.");

            RuleFor(s => s.Date)
                .NotEmpty().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(s => s.CustomerId)
                .NotEqual(Guid.Empty).WithMessage("Customer ID is required.");

            RuleFor(s => s.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(s => s.BranchId)
                .NotEqual(Guid.Empty).WithMessage("Branch ID is required.");

            RuleFor(s => s.BranchName)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

            RuleFor(sale => sale.Status)
                .IsInEnum().WithMessage("Invalid sale status.");

            RuleForEach(sale => sale.Items).SetValidator(new SaleItemValidator());
        }
    }

}
