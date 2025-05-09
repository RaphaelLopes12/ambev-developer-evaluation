using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategories;

/// <summary>
/// Handler for getting all product categories
/// </summary>
public class GetProductCategoriesHandler : IRequestHandler<GetProductCategoriesQuery, List<string>>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the GetProductCategoriesHandler
    /// </summary>
    public GetProductCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the get product categories query
    /// </summary>
    public async Task<List<string>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _productRepository.GetCategoriesAsync();
        return new List<string>(categories);
    }
}
