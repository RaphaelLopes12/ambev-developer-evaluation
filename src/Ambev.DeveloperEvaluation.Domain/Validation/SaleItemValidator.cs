using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for SaleItem entity.
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(si => si.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be a positive number.");

        RuleFor(si => si.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(si => si.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 items of the same product.");

        RuleFor(si => si.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

        RuleFor(si => si.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
            .LessThanOrEqualTo(si => si.UnitPrice * si.Quantity).WithMessage("Discount cannot be greater than the total price.");
    }
}
