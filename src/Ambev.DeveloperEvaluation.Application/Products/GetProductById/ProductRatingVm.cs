namespace Ambev.DeveloperEvaluation.Application.Products.GetProductById;

/// <summary>
/// View model for product rating information
/// </summary>
public class ProductRatingVm
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
