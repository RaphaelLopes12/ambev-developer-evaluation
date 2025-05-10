namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents an individual item in a CreateSaleRequest.
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of product being sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
