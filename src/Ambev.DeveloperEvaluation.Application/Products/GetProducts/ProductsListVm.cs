namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// View model for a paginated list of products
/// </summary>
public class ProductsListVm
{
    /// <summary>
    /// Products data for current page
    /// </summary>
    public ProductDto[] Data { get; set; }

    /// <summary>
    /// Total number of items
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
