using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    /// <summary>
    /// Handler for creating a customer
    /// </summary>
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the CreateCustomerHandler
        /// </summary>
        public CreateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the create customer command
        /// </summary>
        public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Create a new customer entity
            var customer = new Customer(
                request.Name,
                request.Email,
                request.Phone,
                request.Address
            );

            // Validate the customer
            var validationResult = customer.Validate();
            if (!validationResult.IsValid)
            {
                throw new Domain.Exceptions.ValidationException(validationResult);
            }

            // Save the customer
            var createdCustomer = await _customerRepository.AddAsync(customer);

            // Return the result
            return new CreateCustomerResult
            {
                Id = createdCustomer.Id,
                Name = createdCustomer.Name,
                Email = createdCustomer.Email
            };
        }
    }
}
