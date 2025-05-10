using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Handler for updating product stock
/// </summary>
public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateStockHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the UpdateStockHandler
    /// </summary>
    public UpdateStockHandler(
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<UpdateStockHandler> logger)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the update stock command
    /// </summary>
    public async Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.Id} not found.");
        }

        if (request.Quantity < 0)
        {
            throw new ValidationException("Stock quantity cannot be negative.");
        }

        int previousQuantity = product.StockQuantity;
        bool success = await _productRepository.UpdateStockAsync(request.Id, request.Quantity);

        if (success)
        {
            _logger.LogInformation("Stock updated for product ID: {ProductId}, Title: {Title}. Previous: {PreviousQuantity}, New: {NewQuantity}",
                product.Id, product.Title, previousQuantity, request.Quantity);

            await _mediator.Publish(new ProductStockUpdatedNotification
            {
                Id = product.Id,
                Title = product.Title,
                PreviousQuantity = previousQuantity,
                NewQuantity = request.Quantity
            }, cancellationToken);
        }

        return success;
    }
}