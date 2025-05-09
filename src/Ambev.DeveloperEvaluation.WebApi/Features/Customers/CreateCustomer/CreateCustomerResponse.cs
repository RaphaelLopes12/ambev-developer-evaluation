namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

/// <summary>
/// Response model for customer creation
/// </summary>
public class CreateCustomerResponse
{
    /// <summary>
    /// Unique identifier of the created customer
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Name of the created customer
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email of the created customer
    /// </summary>
    public string Email { get; set; }
}
