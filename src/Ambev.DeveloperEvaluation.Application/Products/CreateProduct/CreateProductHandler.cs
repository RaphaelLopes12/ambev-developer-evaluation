using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Handler for creating a product
/// </summary>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the CreateProductHandler
    /// </summary>
    public CreateProductHandler(
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the create product command
    /// </summary>
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        decimal ratingRate = request.Rating?.Rate ?? 0;
        int ratingCount = request.Rating?.Count ?? 0;

        var product = new Product(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image,
            request.StockQuantity,
            ratingRate,
            ratingCount
        );

        var validationResult = product.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult);
        }

        var createdProduct = await _productRepository.AddAsync(product);

        _logger.LogInformation("Product created with ID: {ProductId}, Title: {Title}",
            createdProduct.Id, createdProduct.Title);

        await _mediator.Publish(new ProductCreatedNotification
        {
            Id = createdProduct.Id,
            Title = createdProduct.Title
        }, cancellationToken);

        return new CreateProductResult
        {
            Id = createdProduct.Id,
            Title = createdProduct.Title,
            Category = createdProduct.Category
        };
    }
}