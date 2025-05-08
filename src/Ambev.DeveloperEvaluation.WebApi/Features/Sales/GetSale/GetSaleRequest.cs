namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;


/// <summary>
/// Represents the request model to retrieve a specific sale by ID.
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to retrieve.
    /// </summary>
    public Guid Id { get; set; }
}
