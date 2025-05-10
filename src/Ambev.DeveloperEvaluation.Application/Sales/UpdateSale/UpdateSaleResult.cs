namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Result of a sale update operation.
/// </summary>
public class UpdateSaleResult
{
    /// <summary>
    /// Indicates whether the update was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The unique identifier of the updated sale.
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
    /// The customer name.
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// The branch name.
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// Updated total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The status of the sale.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Updated items in the sale.
    /// </summary>
    public List<UpdatedSaleItemDto> Items { get; set; } = new();

    /// <summary>
    /// When the sale was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Message describing the result of the operation.
    /// </summary>
    public string Message { get; set; }
}