using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.AddRating;

/// <summary>
/// Request model for adding a product rating
/// </summary>
public class AddRatingRequest
{
    /// <summary>
    /// Rating value (0-5)
    /// </summary>
    [Required]
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
    public decimal Rating { get; set; }
}
