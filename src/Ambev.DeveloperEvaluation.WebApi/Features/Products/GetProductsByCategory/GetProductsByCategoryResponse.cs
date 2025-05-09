using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;

/// <summary>
/// Response model for products by category
/// </summary>
public class GetProductsByCategoryResponse
{
    /// <summary>
    /// Category name
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// List of products for the current page
    /// </summary>
    public List<ProductListItem> Products { get; set; } = new List<ProductListItem>();

    /// <summary>
    /// Total number of products in this category
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}
