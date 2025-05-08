using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Handler for cancelling a sale.
    /// </summary>
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of CancelSaleHandler.
        /// </summary>
        /// <param name="saleRepository">Sale repository</param>
        /// <param name="mediator">Mediator instance for publishing events</param>
        public CancelSaleHandler(ISaleRepository saleRepository, IMediator mediator)
        {
            _saleRepository = saleRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Handles the command to cancel a sale.
        /// </summary>
        /// <param name="request">The command parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the cancellation operation</returns>
        public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
            {
                return new CancelSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = "Sale not found"
                };
            }

            try
            {
                sale.Cancel();
                await _saleRepository.UpdateAsync(sale, cancellationToken);

                await _mediator.Publish(new SaleCancelledNotification { SaleId = sale.Id }, cancellationToken);

                return new CancelSaleResult
                {
                    Success = true,
                    Id = sale.Id,
                    Message = "Sale cancelled successfully"
                };
            }
            catch (System.Exception ex)
            {
                return new CancelSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = $"Failed to cancel sale: {ex.Message}"
                };
            }
        }
    }
}
