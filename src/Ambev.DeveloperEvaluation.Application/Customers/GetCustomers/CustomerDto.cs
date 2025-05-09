namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

/// <summary>
/// Data transfer object for customer information
/// </summary>
public class CustomerDto
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

    /// <summary>
    /// Customer phone
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Customer address
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
