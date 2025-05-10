using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.AddRating;

/// <summary>
/// Validator for the add rating command
/// </summary>
public class AddRatingValidator : AbstractValidator<AddRatingCommand>
{
    /// <summary>
    /// Initializes a new instance of the AddRatingValidator
    /// </summary>
    public AddRatingValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Valid product ID is required.");

        RuleFor(p => p.Rating)
            .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");
    }
}
