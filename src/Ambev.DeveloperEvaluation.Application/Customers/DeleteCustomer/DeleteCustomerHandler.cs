using Ambev.DeveloperEvaluation.Application.Customers.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Handler for deleting a customer
/// </summary>
public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the DeleteCustomerHandler
    /// </summary>
    public DeleteCustomerHandler(ICustomerRepository customerRepository, IMediator mediator)
    {
        _customerRepository = customerRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the delete customer command
    /// </summary>
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {request.Id} not found.");
        }

        var success = await _customerRepository.RemoveAsync(request.Id);
        if (success)
        {
            await _mediator.Publish(new CustomerDeletedNotification { Id = request.Id }, cancellationToken);
        }

        return success;
    }
}
