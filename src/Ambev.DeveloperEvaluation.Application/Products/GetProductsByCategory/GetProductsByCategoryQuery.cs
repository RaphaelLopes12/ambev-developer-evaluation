using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Query to get products by category with pagination and sorting
/// </summary>
public class GetProductsByCategoryQuery : IRequest<ProductsByCategoryVm>
{
    /// <summary>
    /// Category name
    /// </summary>
    public string Category { get; set; }

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
