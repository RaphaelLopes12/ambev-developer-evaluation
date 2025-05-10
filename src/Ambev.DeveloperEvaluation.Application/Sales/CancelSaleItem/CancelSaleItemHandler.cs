using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Handler for cancelling a specific item in a sale.
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleItemHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelSaleItemHandler.
    /// </summary>
    /// <param name="saleRepository">Sale repository</param>
    /// <param name="mediator">Mediator instance for publishing events</param>
    /// <param name="mapper">Automapper instance</param>
    /// <param name="logger">Logger instance</param>
    public CancelSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMediator mediator,
        IMapper mapper,
        ILogger<CancelSaleItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command to cancel a specific item in a sale.
    /// </summary>
    /// <param name="request">The command parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the cancellation operation</returns>
    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling item with ProductID: {ProductId} in sale: {SaleId}",
            request.ProductId, request.SaleId);

        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
        {
            _logger.LogWarning("Sale not found with ID: {SaleId}", request.SaleId);

            return new CancelSaleItemResult
            {
                Success = false,
                SaleId = request.SaleId,
                ProductId = request.ProductId,
                Message = "Sale not found"
            };
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            _logger.LogWarning("Product with ID: {ProductId} not found", request.ProductId);
            return new CancelSaleItemResult
            {
                Success = false,
                SaleId = request.SaleId,
                ProductId = request.ProductId,
                Message = "Product not found"
            };
        }

        try
        {
            var item = sale.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
            {
                _logger.LogWarning("Item with ProductId: {ProductId} not found in sale: {SaleId}",
                    request.ProductId, request.SaleId);

                return new CancelSaleItemResult
                {
                    Success = false,
                    SaleId = request.SaleId,
                    ProductId = request.ProductId,
                    Message = "Item not found in sale"
                };
            }

            if (item.IsCancelled)
            {
                _logger.LogWarning("Item with ProductId: {ProductId} is already cancelled in sale: {SaleId}",
                    request.ProductId, request.SaleId);

                return new CancelSaleItemResult
                {
                    Success = false,
                    SaleId = request.SaleId,
                    ProductId = request.ProductId,
                    Message = "Item is already cancelled"
                };
            }

            sale.CancelItem(request.ProductId);

            item = sale.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null || !item.IsCancelled)
            {
                throw new InvalidOperationException("Failed to cancel item");
            }

            var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

            _logger.LogInformation("Item with ProductId: {ProductId} cancelled successfully in sale: {SaleId}. New total amount: {TotalAmount}",
                request.ProductId, request.SaleId, updatedSale.TotalAmount);

            await _mediator.Publish(new ItemCancelledNotification
            {
                SaleId = sale.Id,
                ProductId = request.ProductId
            }, cancellationToken);

            return new CancelSaleItemResult
            {
                Success = true,
                SaleId = sale.Id,
                ProductId = request.ProductId,
                NewTotalAmount = updatedSale.TotalAmount,
                Message = "Item cancelled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling item with ProductId: {ProductId} in sale: {SaleId}: {ErrorMessage}",
                request.ProductId, request.SaleId, ex.Message);

            return new CancelSaleItemResult
            {
                Success = false,
                SaleId = request.SaleId,
                ProductId = request.ProductId,
                Message = $"Failed to cancel item: {ex.Message}"
            };
        }
    }
}