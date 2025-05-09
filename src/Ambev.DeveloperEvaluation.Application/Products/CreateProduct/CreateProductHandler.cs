using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Handler for creating a product
/// </summary>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the CreateProductHandler
    /// </summary>
    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the create product command
    /// </summary>
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image,
            request.StockQuantity
        );

        var validationResult = product.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult);
        }

        var createdProduct = await _productRepository.AddAsync(product);

        return new CreateProductResult
        {
            Id = createdProduct.Id,
            Title = createdProduct.Title,
            Category = createdProduct.Category
        };
    }
}
