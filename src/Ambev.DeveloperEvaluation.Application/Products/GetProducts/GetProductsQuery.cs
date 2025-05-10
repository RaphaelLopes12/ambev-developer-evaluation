using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Query to get all products with pagination and sorting
/// </summary>
public class GetProductsQuery : IRequest<ProductsListVm>
{
    /// <summary>
    /// Page number (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Items per page
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Ordering expression (e.g. "price desc, title asc")
    /// </summary>
    public string Order { get; set; }
}
