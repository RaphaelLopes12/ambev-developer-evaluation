using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Handler for getting products by category with pagination and sorting
/// </summary>
public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, ProductsByCategoryVm>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetProductsByCategoryHandler
    /// </summary>
    public GetProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get products by category query
    /// </summary>
    public async Task<ProductsByCategoryVm> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var (products, totalItems, currentPage, totalPages) =
            await _productRepository.GetByCategoryAsync(request.Category, request.Page, request.Size, request.Order);

        var productDtos = _mapper.Map<ProductDto[]>(products.ToArray());

        return new ProductsByCategoryVm
        {
            Category = request.Category,
            Data = productDtos,
            TotalItems = totalItems,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
    }
}
