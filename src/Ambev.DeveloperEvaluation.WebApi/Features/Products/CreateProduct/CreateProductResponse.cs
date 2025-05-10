namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Response model for product creation
/// </summary>
public class CreateProductResponse
{
    /// <summary>
    /// ID of the created product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the created product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Price of the created product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Category of the created product
    /// </summary>
    public string Category { get; set; }
}
