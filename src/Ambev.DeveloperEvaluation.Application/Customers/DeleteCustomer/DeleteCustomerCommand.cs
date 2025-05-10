using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Command to delete a customer
/// </summary>
public class DeleteCustomerCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the customer to delete
    /// </summary>
    public string Id { get; set; }
}
