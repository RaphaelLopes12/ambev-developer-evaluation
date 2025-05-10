namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Data transfer object for product information
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Product ID
    /// </summary>
    public Guid Id { get; set; }

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
    /// Product rating information
    /// </summary>
    public RatingDto Rating { get; set; }
}
