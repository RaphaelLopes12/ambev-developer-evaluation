using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

/// <summary>
/// Handler for getting all customers
/// </summary>
public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetCustomersHandler
    /// </summary>
    public GetCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get customers query
    /// </summary>
    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<List<CustomerDto>>(customers);
    }
}
