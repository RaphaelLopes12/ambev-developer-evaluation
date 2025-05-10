using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.AddRating;

/// <summary>
/// Handler for adding a rating to a product
/// </summary>
public class AddRatingHandler : IRequestHandler<AddRatingCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<AddRatingHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the AddRatingHandler
    /// </summary>
    public AddRatingHandler(
        IProductRepository productRepository,
        ILogger<AddRatingHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the add rating command
    /// </summary>
    public async Task<bool> Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        if (request.Rating < 0 || request.Rating > 5)
        {
            throw new ValidationException("Rating must be between 0 and 5.");
        }

        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.Id} not found.");
        }

        bool success = await _productRepository.AddRatingAsync(request.Id, request.Rating);

        if (success)
        {
            _logger.LogInformation("Rating added to product ID: {ProductId}, Rating: {Rating}",
                request.Id, request.Rating);
        }

        return success;
    }
}