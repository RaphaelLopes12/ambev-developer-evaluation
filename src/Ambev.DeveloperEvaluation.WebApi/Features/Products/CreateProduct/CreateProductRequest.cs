using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Request model for creating a product
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Title/name of the product
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Price of the product
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    /// <summary>
    /// Detailed description of the product
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Category of the product
    /// </summary>
    [Required]
    public string Category { get; set; }

    /// <summary>
    /// URL to the product image
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Initial stock quantity
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int StockQuantity { get; set; }

    /// <summary>
    /// Initial rating information (optional)
    /// </summary>
    public CreateProductRatingRequest Rating { get; set; }
}
