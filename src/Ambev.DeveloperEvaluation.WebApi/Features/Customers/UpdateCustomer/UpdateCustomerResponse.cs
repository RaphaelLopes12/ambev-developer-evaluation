namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

/// <summary>
/// Response model for customer update
/// </summary>
public class UpdateCustomerResponse
{
    /// <summary>
    /// Unique identifier of the updated customer
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Updated name of the customer
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Updated email of the customer
    /// </summary>
    public string Email { get; set; }
}
