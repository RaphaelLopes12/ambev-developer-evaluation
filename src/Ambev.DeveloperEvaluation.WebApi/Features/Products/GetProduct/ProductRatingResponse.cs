namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Response model for product rating information
/// </summary>
public class ProductRatingResponse
{
    /// <summary>
    /// Average rating value (0-5)
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Number of ratings
    /// </summary>
    public int Count { get; set; }
}
