namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Response model for updating a sale.
/// </summary>
public class UpdateSaleResponse
{
    /// <summary>
    /// The identifier of the updated sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The business number of the sale.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// The date of the sale.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The status of the sale.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// When the sale was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// The items in the sale.
    /// </summary>
    public List<UpdatedSaleItemResponse> Items { get; set; } = new();
}
