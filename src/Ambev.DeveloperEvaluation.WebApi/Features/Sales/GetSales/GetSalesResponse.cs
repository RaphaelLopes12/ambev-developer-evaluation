namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Response for GetSales endpoint.
/// </summary>
public class GetSalesResponse
{
    /// <summary>
    /// List of sales for the current page.
    /// </summary>
    public List<GetSalesItemResponse> Items { get; set; } = new();

    /// <summary>
    /// Total number of sales.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages { get; set; }
}
