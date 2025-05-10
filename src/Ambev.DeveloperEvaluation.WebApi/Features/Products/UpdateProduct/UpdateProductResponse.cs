namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Response model for product update
/// </summary>
public class UpdateProductResponse
{
    /// <summary>
    /// Success flag
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message describing the result
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// ID of the updated product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Updated title of the product
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
