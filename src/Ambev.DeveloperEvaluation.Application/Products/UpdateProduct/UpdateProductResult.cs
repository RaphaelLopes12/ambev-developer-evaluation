namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Result of product update operation
/// </summary>
public class UpdateProductResult
{
    /// <summary>
    /// ID of the updated product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Updated title/name of the product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Updated price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Updated category of the product
    /// </summary>
    public string Category { get; set; }
}
