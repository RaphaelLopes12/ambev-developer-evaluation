namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Request to update a sale item.
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// Identifier of the existing item (if updating an existing item).
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}