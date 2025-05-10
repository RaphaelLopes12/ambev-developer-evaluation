namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Request to update a sale.
/// </summary>
public class UpdateSaleRequest
{
    /// <summary>
    /// Sale number (business identifier).
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Date when the sale was made.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Customer identifier.
    /// </summary>
    public string CustomerId { get; set; }

    /// <summary>
    /// Customer name (denormalized).
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Branch identifier.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Branch name (denormalized).
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// List of items to update.
    /// </summary>
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}
