using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

/// <summary>
/// Request model for getting a customer by ID
/// </summary>
public class GetCustomerRequest
{
    /// <summary>
    /// ID of the customer to get
    /// </summary>
    [Required]
    public string Id { get; set; }
}
