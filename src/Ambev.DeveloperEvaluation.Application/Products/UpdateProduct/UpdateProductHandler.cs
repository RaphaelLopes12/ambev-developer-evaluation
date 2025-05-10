using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for updating a product
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the UpdateProductHandler
    /// </summary>
    public UpdateProductHandler(
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the update product command
    /// </summary>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.Id} not found.");
        }

        product.UpdateDetails(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image
        );

        var validationResult = product.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult);
        }

        var success = await _productRepository.UpdateDetailsAsync(
            request.Id,
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image
        );

        if (!success)
        {
            throw new ApplicationException($"Failed to update product with ID {request.Id}.");
        }

        var updatedProduct = await _productRepository.GetByIdAsync(request.Id);

        _logger.LogInformation("Product updated with ID: {ProductId}, Title: {Title}",
            updatedProduct.Id, updatedProduct.Title);

        await _mediator.Publish(new ProductUpdatedNotification
        {
            Id = updatedProduct.Id,
            Title = updatedProduct.Title
        }, cancellationToken);

        return new UpdateProductResult
        {
            Id = updatedProduct.Id,
            Title = updatedProduct.Title,
            Price = updatedProduct.Price,
            Category = updatedProduct.Category
        };
    }
}