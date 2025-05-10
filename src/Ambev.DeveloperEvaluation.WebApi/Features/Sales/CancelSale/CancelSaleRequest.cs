namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Request model to cancel a sale.
/// </summary>
public class CancelSaleRequest
{
    /// <summary>
    /// The sale ID to be canceled.
    /// </summary>
    public Guid Id { get; set; }
}
