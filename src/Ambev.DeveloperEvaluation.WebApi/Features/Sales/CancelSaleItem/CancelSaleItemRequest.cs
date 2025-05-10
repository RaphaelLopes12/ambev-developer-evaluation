namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// Request model to cancel a sale item.
/// </summary>
public class CancelSaleItemRequest
{
    /// <summary>
    /// The sale ID containing the item.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The product ID to be canceled.
    /// </summary>
    public Guid ProductId { get; set; }
}
