using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Request model for product rating information
/// </summary>
public class CreateProductRatingRequest
{
    /// <summary>
    /// Initial rating value (0-5)
    /// </summary>
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
    public decimal Rate { get; set; }

    /// <summary>
    /// Initial number of ratings
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Rating count cannot be negative")]
    public int Count { get; set; }
}
