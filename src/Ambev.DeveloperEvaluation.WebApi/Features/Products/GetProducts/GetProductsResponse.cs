namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Response model for product list
/// </summary>
public class GetProductsResponse
{
    /// <summary>
    /// List of products for the current page
    /// </summary>
    public List<ProductListItem> Products { get; set; } = new List<ProductListItem>();

    /// <summary>
    /// Total number of products
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
