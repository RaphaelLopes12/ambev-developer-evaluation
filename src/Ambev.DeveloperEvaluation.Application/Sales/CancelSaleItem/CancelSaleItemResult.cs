namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Result of a sale item cancellation.
/// </summary>
public class CancelSaleItemResult
{
    /// <summary>
    /// Indicates whether the cancellation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Updated total amount of the sale.
    /// </summary>
    public decimal NewTotalAmount { get; set; }

    /// <summary>
    /// Message describing the result of the operation.
    /// </summary>
    public string Message { get; set; }
}
