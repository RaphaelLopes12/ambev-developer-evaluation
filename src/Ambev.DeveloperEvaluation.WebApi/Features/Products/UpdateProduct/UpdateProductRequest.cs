using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Request model for updating a product
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Updated title/name of the product
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Updated price of the product
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    /// <summary>
    /// Updated description of the product
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Updated category of the product
    /// </summary>
    [Required]
    public string Category { get; set; }

    /// <summary>
    /// Updated URL to the product image
    /// </summary>
    public string Image { get; set; }
}
