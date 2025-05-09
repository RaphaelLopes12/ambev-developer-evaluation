using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

/// <summary>
/// Request model for updating a customer
/// </summary>
public class UpdateCustomerRequest
{
    /// <summary>
    /// Updated full name of the customer
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Updated email address of the customer
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Updated phone number of the customer
    /// </summary>
    [Required]
    public string Phone { get; set; }

    /// <summary>
    /// Updated address of the customer
    /// </summary>
    [Required]
    public string Address { get; set; }
}
