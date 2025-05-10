using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for deleting a sale.
/// </summary>
public class DeleteSaleCommand : IRequest<bool>
{
    /// <summary>
    /// The identifier of the sale to delete.
    /// </summary>
    public Guid Id { get; set; }
}
