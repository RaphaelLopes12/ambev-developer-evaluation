using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Command to create a new product
/// </summary>
public class CreateProductCommand : IRequest<CreateProductResult>
{
    /// <summary>
    /// Title/name of the product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Detailed description of the product
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Category of the product
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// URL to the product image
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Initial stock quantity
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Initial rating information (optional)
    /// </summary>
    public RatingDto Rating { get; set; }
}
