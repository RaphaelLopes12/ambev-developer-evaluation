using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Command to update an existing product
/// </summary>
public class UpdateProductCommand : IRequest<UpdateProductResult>
{
    /// <summary>
    /// ID of the product to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Updated title/name of the product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Updated price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Updated detailed description of the product
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Updated category of the product
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Updated URL to the product image
    /// </summary>
    public string Image { get; set; }
}
