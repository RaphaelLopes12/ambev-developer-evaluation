using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductById;

/// <summary>
/// Validator for the get product by ID query
/// </summary>
public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the GetProductByIdValidator
    /// </summary>
    public GetProductByIdValidator()
    {
        RuleFor(p=> p.Id)
            .NotEmpty().WithMessage("Valid product ID is required.");
    }
}
