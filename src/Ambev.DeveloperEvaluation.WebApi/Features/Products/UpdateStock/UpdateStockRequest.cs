using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// Request model for updating product stock
/// </summary>
public class UpdateStockRequest
{
    /// <summary>
    /// New stock quantity
    /// </summary>
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int Quantity { get; set; }
}
