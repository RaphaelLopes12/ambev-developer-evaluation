﻿using Ambev.DeveloperEvaluation.Application.Customers.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Handler for updating a customer
/// </summary>
public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the UpdateCustomerHandler
    /// </summary>
    public UpdateCustomerHandler(ICustomerRepository customerRepository, IMediator mediator, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the update customer command
    /// </summary>
    public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {request.Id} not found.");
        }

        customer.UpdateDetails(
            request.Name,
            request.Email,
            request.Phone,
            request.Address
        );

        var validationResult = customer.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult);
        }

        await _customerRepository.UpdateAsync(customer);

        await _mediator.Publish(new CustomerUpdatedNotification { Id = request.Id }, cancellationToken);

        return new UpdateCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }
}
