using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductById;

/// <summary>
/// Query to get a product by ID
/// </summary>
public class GetProductByIdQuery : IRequest<ProductDetailVm>
{
    /// <summary>
    /// ID of the product to get
    /// </summary>
    public Guid Id { get; set; }
}
