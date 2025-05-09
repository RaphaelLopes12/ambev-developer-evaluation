using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Product list item information
/// </summary>
public class ProductListItem
{
    /// <summary>
    /// ID of the product
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the product
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Category of the product
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// URL to the product image
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Rating information
    /// </summary>
    public ProductRatingResponse Rating { get; set; }
}
