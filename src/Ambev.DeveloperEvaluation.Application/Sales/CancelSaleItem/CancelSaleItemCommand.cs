using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Command to cancel a specific item in a sale.
/// </summary>
public class CancelSaleItemCommand : IRequest<CancelSaleItemResult>
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The unique identifier of the product to cancel.
    /// </summary>
    public Guid ProductId { get; set; }
}
