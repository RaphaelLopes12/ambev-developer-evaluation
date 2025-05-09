namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Data transfer object for product rating information
/// </summary>
public class RatingDto
{
    /// <summary>
    /// Average rating (0-5)
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Number of ratings
    /// </summary>
    public int Count { get; set; }
}
