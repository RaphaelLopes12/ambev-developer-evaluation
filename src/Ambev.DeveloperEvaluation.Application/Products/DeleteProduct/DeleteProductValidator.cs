using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Validator for the delete product command
/// </summary>
public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteProductValidator
    /// </summary>
    public DeleteProductValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("Valid product ID is required.");
    }
}
