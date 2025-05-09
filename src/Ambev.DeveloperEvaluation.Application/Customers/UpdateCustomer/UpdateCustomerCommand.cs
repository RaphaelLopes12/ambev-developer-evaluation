using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Command to update an existing customer
/// </summary>
public class UpdateCustomerCommand : IRequest<UpdateCustomerResult>
{
    /// <summary>
    /// ID of the customer to update
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Updated full name of the customer
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Updated email address of the customer
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Updated phone number of the customer
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Updated address of the customer
    /// </summary>
    public string Address { get; set; }
}
