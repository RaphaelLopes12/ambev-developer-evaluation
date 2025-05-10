namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductCategories;

/// <summary>
/// Response model for product categories
/// </summary>
public class GetProductCategoriesResponse
{
    /// <summary>
    /// List of category names
    /// </summary>
    public List<string> Categories { get; set; } = new List<string>();
}
