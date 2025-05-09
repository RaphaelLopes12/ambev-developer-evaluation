namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Result of customer creation operation
/// </summary>
public class CreateCustomerResult
{
    /// <summary>
    /// Customer ID
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Customer email
    /// </summary>
    public string Email { get; set; }
}
