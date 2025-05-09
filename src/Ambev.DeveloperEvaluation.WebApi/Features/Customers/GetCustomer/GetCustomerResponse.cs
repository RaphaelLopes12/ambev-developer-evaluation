namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

/// <summary>
/// Response model for customer details
/// </summary>
public class GetCustomerResponse
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

    /// <summary>
    /// Address of the customer
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
