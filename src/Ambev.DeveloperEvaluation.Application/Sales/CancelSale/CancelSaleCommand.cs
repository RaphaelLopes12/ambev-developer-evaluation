using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Command to cancel a sale.
    /// </summary>
    public class CancelSaleCommand : IRequest<CancelSaleResult>
    {
        /// <summary>
        /// The unique identifier of the sale to cancel.
        /// </summary>
        public Guid Id { get; set; }
    }
}
