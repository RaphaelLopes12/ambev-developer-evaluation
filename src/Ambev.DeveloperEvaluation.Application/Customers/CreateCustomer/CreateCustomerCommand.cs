using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Command to create a new customer
/// </summary>
public class CreateCustomerCommand : IRequest<CreateCustomerResult>
{
    /// <summary>
    /// Full name of the customer
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email address of the customer
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Phone number of the customer
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Address of the customer
    /// </summary>
    public string Address { get; set; }
}
