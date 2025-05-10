using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Command to update the stock of a product
/// </summary>
public class UpdateStockCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the product to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// New stock quantity
    /// </summary>
    public int Quantity { get; set; }
}
