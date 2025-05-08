namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// Response for cancelling a sale item.
/// </summary>
public class CancelSaleItemResponse
{
    /// <summary>
    /// Indicates whether the cancellation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The identifier of the product that was cancelled.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The new total amount of the sale after cancellation.
    /// </summary>
    public decimal NewTotalAmount { get; set; }

    /// <summary>
    /// A message describing the result of the operation.
    /// </summary>
    public string Message { get; set; }
}
