namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Response model for product details
/// </summary>
public class GetProductResponse
{
    /// <summary>
    /// ID of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Description of the product
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Category of the product
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// URL to the product image
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Current stock quantity
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Rating information
    /// </summary>
    public ProductRatingResponse Rating { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
