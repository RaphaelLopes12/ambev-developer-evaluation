using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Handler for getting products with pagination and sorting
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsQuery, ProductsListVm>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetProductsHandler
    /// </summary>
    public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get products query
    /// </summary>
    public async Task<ProductsListVm> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var (products, totalItems, currentPage, totalPages) =
            await _productRepository.GetAllAsync(request.Page, request.Size, request.Order);

        var productDtos = _mapper.Map<ProductDto[]>(products.ToArray());

        return new ProductsListVm
        {
            Data = productDtos,
            TotalItems = totalItems,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
    }
}
