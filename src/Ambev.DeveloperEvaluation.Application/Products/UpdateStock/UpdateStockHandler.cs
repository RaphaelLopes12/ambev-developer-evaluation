using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Handler for updating the stock of a product
/// </summary>
public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, bool>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the UpdateStockHandler
    /// </summary>
    public UpdateStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
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

        return await _productRepository.UpdateStockAsync(request.Id, request.Quantity);
    }
}
