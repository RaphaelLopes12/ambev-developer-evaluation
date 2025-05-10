using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for the create product command
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductValidator
    /// </summary>
    public CreateProductValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100).WithMessage("Category cannot exceed 100 characters.");

        RuleFor(p => p.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
    }
}
