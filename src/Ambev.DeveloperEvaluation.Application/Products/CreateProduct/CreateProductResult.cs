namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Result of product creation operation
/// </summary>
public class CreateProductResult
{
    /// <summary>
    /// ID of the created product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title/name of the created product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Category of the created product
    /// </summary>
    public string Category { get; set; }
}
