using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for deleting a product
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the DeleteProductHandler
    /// </summary>
    public DeleteProductHandler(
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the delete product command
    /// </summary>
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found for deletion", request.Id);
            return false;
        }

        string productTitle = product.Title;

        bool success = await _productRepository.RemoveAsync(request.Id);

        if (success)
        {
            _logger.LogInformation("Product deleted with ID: {ProductId}, Title: {Title}",
                request.Id, productTitle);

            await _mediator.Publish(new ProductDeletedNotification
            {
                Id = request.Id,
                Title = productTitle
            }, cancellationToken);
        }

        return success;
    }
}