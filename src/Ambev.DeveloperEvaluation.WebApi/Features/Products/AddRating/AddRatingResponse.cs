namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.AddRating;

/// <summary>
/// Response model for adding a product rating
/// </summary>
public class AddRatingResponse
{
    /// <summary>
    /// Success flag
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message describing the result
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// ID of the product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// New average rating
    /// </summary>
    public decimal NewRating { get; set; }

    /// <summary>
    /// New rating count
    /// </summary>
    public int RatingCount { get; set; }
}
