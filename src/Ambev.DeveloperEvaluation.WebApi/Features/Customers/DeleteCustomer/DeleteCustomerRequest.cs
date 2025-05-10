using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;

/// <summary>
/// Request model for deleting a customer
/// </summary>
public class DeleteCustomerRequest
{
    /// <summary>
    /// ID of the customer to delete
    /// </summary>
    [Required]
    public string Id { get; set; }
}
