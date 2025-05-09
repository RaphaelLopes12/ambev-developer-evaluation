using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.AddRating;

/// <summary>
/// Handler for adding a rating to a product
/// </summary>
public class AddRatingHandler : IRequestHandler<AddRatingCommand, bool>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the AddRatingHandler
    /// </summary>
    public AddRatingHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the add rating command
    /// </summary>
    public async Task<bool> Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.Id} not found.");
        }

        return await _productRepository.AddRatingAsync(request.Id, request.Rating);
    }
}
