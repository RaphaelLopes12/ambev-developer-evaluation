using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.Notifications;

/// <summary>
/// Handler for all customer-related events.
/// </summary>
public class CustomerEventNotificationHandler :
    INotificationHandler<CustomerCreatedNotification>,
    INotificationHandler<CustomerUpdatedNotification>,
    INotificationHandler<CustomerDeletedNotification>
{
    private readonly ILogger<CustomerEventNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CustomerEventNotificationHandler.
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public CustomerEventNotificationHandler(ILogger<CustomerEventNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles customer created notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(CustomerCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer created: {CustomerId}", notification.Id);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("customer.created", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles customer updated notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(CustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer updated: {CustomerId}", notification.Id);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("customer.updated", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles customer deleted notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(CustomerDeletedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer deleted: {CustomerId}", notification.Id);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("customer.deleted", notification);

        return Task.CompletedTask;
    }
}
