using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for updating a product
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the UpdateProductHandler
    /// </summary>
    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
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

        var result = await _productRepository.UpdateDetailsAsync(
            request.Id,
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image
        );

        if (!result)
        {
            throw new ApplicationException($"Failed to update product with ID {request.Id}.");
        }

        var updatedProduct = await _productRepository.GetByIdAsync(request.Id);

        return new UpdateProductResult
        {
            Id = updatedProduct.Id,
            Title = updatedProduct.Title,
            Price = updatedProduct.Price,
            Category = updatedProduct.Category
        };
    }
}
