using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command to delete a product
/// </summary>
public class DeleteProductCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the product to delete
    /// </summary>
    public Guid Id { get; set; }
}
