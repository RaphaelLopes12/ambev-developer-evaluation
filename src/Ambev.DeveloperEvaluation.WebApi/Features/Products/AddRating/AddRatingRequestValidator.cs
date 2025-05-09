using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.AddRating;

/// <summary>
/// Validator for rating requests
/// </summary>
public class AddRatingRequestValidator : AbstractValidator<AddRatingRequest>
{
    public AddRatingRequestValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5");
    }
}
