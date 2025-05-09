namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Result of customer update operation
/// </summary>
public class UpdateCustomerResult
{
    /// <summary>
    /// Customer ID
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Updated customer name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Updated customer email
    /// </summary>
    public string Email { get; set; }
}
