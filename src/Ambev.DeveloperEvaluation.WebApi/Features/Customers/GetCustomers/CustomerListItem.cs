namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;

/// <summary>
/// Customer list item information
/// </summary>
public class CustomerListItem
{
    /// <summary>
    /// Unique identifier of the customer
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Full name of the customer
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email address of the customer
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Phone number of the customer
    /// </summary>
    public string Phone { get; set; }
}
