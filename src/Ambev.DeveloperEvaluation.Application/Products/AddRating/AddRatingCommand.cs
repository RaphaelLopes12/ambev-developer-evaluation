using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.AddRating;

/// <summary>
/// Command to add a rating to a product
/// </summary>
public class AddRatingCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the product to rate
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Rating value (0-5)
    /// </summary>
    public decimal Rating { get; set; }
}
