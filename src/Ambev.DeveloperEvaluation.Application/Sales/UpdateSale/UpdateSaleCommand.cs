using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command to update an existing sale.
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// The unique identifier of the sale to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Date when the sale was made.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Customer identifier.
    /// </summary>
    public string CustomerId { get; set; }

    /// <summary>
    /// Customer name.
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Branch identifier.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Branch name.
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// List of updated items.
    /// </summary>
    public List<UpdateSaleItemDto> Items { get; set; } = new();
}
