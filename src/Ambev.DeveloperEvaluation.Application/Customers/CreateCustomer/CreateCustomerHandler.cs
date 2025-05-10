using Ambev.DeveloperEvaluation.Application.Customers.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Handler for creating a customer
/// </summary>
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CreateCustomerHandler
    /// </summary>
    public CreateCustomerHandler(ICustomerRepository customerRepository, IMediator mediator, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the create customer command
    /// </summary>
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer(
            request.Name,
            request.Email,
            request.Phone,
            request.Address
        );

        var validationResult = customer.Validate();
        if (!validationResult.IsValid)
        {
            throw new Domain.Exceptions.ValidationException(validationResult);
        }

        var createdCustomer = await _customerRepository.AddAsync(customer);

        await _mediator.Publish(new CustomerCreatedNotification { Id = customer.Id }, cancellationToken);

        return new CreateCustomerResult
        {
            Id = createdCustomer.Id,
            Name = createdCustomer.Name,
            Email = createdCustomer.Email
        };
    }
}
