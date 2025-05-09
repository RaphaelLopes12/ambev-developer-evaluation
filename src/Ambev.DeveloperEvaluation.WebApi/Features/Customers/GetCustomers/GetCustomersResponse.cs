namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;

/// <summary>
/// Response model for customer list
/// </summary>
public class GetCustomersResponse
{
    /// <summary>
    /// List of customers
    /// </summary>
    public List<CustomerListItem> Customers { get; set; } = new List<CustomerListItem>();
}
