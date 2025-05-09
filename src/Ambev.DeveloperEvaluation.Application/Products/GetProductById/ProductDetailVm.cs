namespace Ambev.DeveloperEvaluation.Application.Products.GetProductById;

/// <summary>
/// View model for detailed product information
/// </summary>
public class ProductDetailVm
{
    /// <summary>
    /// Product ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product title/name
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Product description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Product category
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Product image URL
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Current stock quantity
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Product rating information
    /// </summary>
    public ProductRatingVm Rating { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
