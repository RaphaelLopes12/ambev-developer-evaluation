using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for deleting a sale.
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">Sale repository</param>
    /// <param name="productRepository">Product repository</param>
    /// <param name="logger">Logger instance</param>
    public DeleteSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command for deleting a sale.
    /// </summary>
    /// <param name="request">The command parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false otherwise</returns>
    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting sale with ID: {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            _logger.LogWarning("Sale with ID: {SaleId} not found for deletion", request.Id);
            return false;
        }

        try
        {
            var itemsToRestoreStock = sale.Items
                .Where(i => !i.IsCancelled)
                .ToList();

            var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);

            if (success)
            {
                _logger.LogInformation("Sale with ID: {SaleId} deleted successfully", request.Id);

                await _mediator.Publish(new SaleDeletedNotification { SaleId = request.Id }, cancellationToken);

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
                                "Product {ProductId} not found when trying to restore stock during sale deletion",
                                item.ProductId
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error restoring stock for product {ProductId} during sale deletion: {ErrorMessage}",
                            item.ProductId,
                            ex.Message
                        );
                    }
                }

                return true;
            }
            else
            {
                _logger.LogWarning("Failed to delete sale with ID: {SaleId}", request.Id);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sale with ID {SaleId}: {ErrorMessage}", request.Id, ex.Message);
            return false;
        }
    }
}