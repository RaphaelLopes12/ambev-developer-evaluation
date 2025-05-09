using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Handler for getting a customer by ID
/// </summary>
public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDetailDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetCustomerByIdHandler
    /// </summary>
    public GetCustomerByIdHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get customer by ID query
    /// </summary>
    public async Task<CustomerDetailDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {request.Id} not found.");
        }

        return _mapper.Map<CustomerDetailDto>(customer);
    }
}
