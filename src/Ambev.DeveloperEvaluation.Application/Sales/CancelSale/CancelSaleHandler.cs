using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for cancelling a sale.
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CancelSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelSaleHandler.
    /// </summary>
    /// <param name="saleRepository">Sale repository</param>
    /// <param name="productRepository">Product repository</param>
    /// <param name="mediator">Mediator instance for publishing events</param>
    /// <param name="logger">Logger instance</param>
    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command to cancel a sale.
    /// </summary>
    /// <param name="request">The command parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the cancellation operation</returns>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to cancel sale with ID: {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            _logger.LogWarning("Sale not found with ID: {SaleId}", request.Id);
            return new CancelSaleResult
            {
                Success = false,
                Id = request.Id,
                Message = "Sale not found"
            };
        }

        try
        {
            if (sale.Status.ToString() == "Cancelled")
            {
                _logger.LogWarning("Sale with ID: {SaleId} is already cancelled", request.Id);
                return new CancelSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = "Sale is already cancelled"
                };
            }

            var itemsToRestoreStock = sale.Items
                .Where(i => !i.IsCancelled)
                .ToList();

            sale.Cancel();

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            foreach (var item in itemsToRestoreStock)
            {
                try
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        await _productRepository.UpdateStockAsync(
                            item.ProductId,
                            product.StockQuantity + item.Quantity
                        );

                        _logger.LogInformation(
                            "Restored stock for product {ProductId}. Previous: {PreviousStock}, Added: {Quantity}, New: {NewStock}",
                            item.ProductId,
                            product.StockQuantity,
                            item.Quantity,
                            product.StockQuantity + item.Quantity
                        );
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Product {ProductId} not found when trying to restore stock during sale cancellation",
                            item.ProductId
                        );
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error restoring stock for product {ProductId} during sale cancellation: {ErrorMessage}",
                        item.ProductId,
                        ex.Message
                    );
                }
            }

            await _mediator.Publish(new SaleCancelledNotification { SaleId = sale.Id }, cancellationToken);

            _logger.LogInformation("Sale with ID: {SaleId} cancelled successfully", sale.Id);

            return new CancelSaleResult
            {
                Success = true,
                Id = sale.Id,
                Message = "Sale cancelled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling sale {SaleId}: {ErrorMessage}", request.Id, ex.Message);

            return new CancelSaleResult
            {
                Success = false,
                Id = request.Id,
                Message = $"Failed to cancel sale: {ex.Message}"
            };
        }
    }
}