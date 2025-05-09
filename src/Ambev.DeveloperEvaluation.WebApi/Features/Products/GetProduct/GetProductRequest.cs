using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Request model for getting a product by ID
/// </summary>
public class GetProductRequest
{
    /// <summary>
    /// ID of the product to get
    /// </summary>
    [Required]
    public int Id { get; set; }
}
