using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Request model for getting products with pagination and sorting
/// </summary>
public class GetProductsRequest
{
    /// <summary>
    /// Page number (starts from 1)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Items per page
    /// </summary>
    [Range(1, 100, ErrorMessage = "Size must be between 1 and 100")]
    public int Size { get; set; } = 10;

    /// <summary>
    /// Ordering expression (e.g. "price desc, title asc")
    /// </summary>
    public string Order { get; set; }
}
