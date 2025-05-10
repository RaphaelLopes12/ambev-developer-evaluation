namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// DTO for updating a sale item.
/// </summary>
public class UpdateSaleItemDto
{
    /// <summary>
    /// Unique identifier of the item (if existing).
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product name.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
