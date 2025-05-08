using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request for retrieving a paginated list of sales.
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    public int PageSize { get; set; } = 10;
}
