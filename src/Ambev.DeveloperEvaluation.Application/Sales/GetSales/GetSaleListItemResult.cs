namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// List item result for sales.
/// </summary>
public class GetSaleListItemResult
{
    /// <summary>
    /// The sale ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// The sale date.
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
    /// The total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The status of the sale.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// The number of items in the sale.
    /// </summary>
    public int ItemCount { get; set; }
}
