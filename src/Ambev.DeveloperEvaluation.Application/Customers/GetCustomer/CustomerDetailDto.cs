namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Data transfer object for detailed customer information
/// </summary>
public class CustomerDetailDto
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
    /// PostgreSQL ID if linked
    /// </summary>
    public int? PostgreSQLId { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
