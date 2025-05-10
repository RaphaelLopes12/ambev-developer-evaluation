using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

/// <summary>
/// Request model for creating a customer
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>
    /// Full name of the customer
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Email address of the customer
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Phone number of the customer
    /// </summary>
    [Required]
    public string Phone { get; set; }

    /// <summary>
    /// Address of the customer
    /// </summary>
    [Required]
    public string Address { get; set; }
}
