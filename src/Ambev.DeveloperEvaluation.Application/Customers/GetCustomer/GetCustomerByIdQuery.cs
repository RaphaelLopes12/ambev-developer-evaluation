using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Query to get a customer by ID
/// </summary>
public class GetCustomerByIdQuery : IRequest<CustomerDetailDto>
{
    /// <summary>
    /// ID of the customer to get
    /// </summary>
    public string Id { get; set; }
}
