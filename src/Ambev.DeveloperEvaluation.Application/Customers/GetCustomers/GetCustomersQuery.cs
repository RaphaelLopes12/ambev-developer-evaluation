using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

/// <summary>
/// Query to get all customers
/// </summary>
public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
}
