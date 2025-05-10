namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// Response model for stock update
/// </summary>
public class UpdateStockResponse
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
    /// ID of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// New stock quantity
    /// </summary>
    public int Quantity { get; set; }
}
