using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.Notifications;

/// <summary>
/// Handler for all branch-related events.
/// </summary>
public class BranchEventNotificationHandler :
    INotificationHandler<BranchCreatedNotification>,
    INotificationHandler<BranchUpdatedNotification>,
    INotificationHandler<BranchActivatedNotification>,
    INotificationHandler<BranchDeactivatedNotification>,
    INotificationHandler<BranchDeletedNotification>
{
    private readonly ILogger<BranchEventNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of BranchEventNotificationHandler.
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public BranchEventNotificationHandler(ILogger<BranchEventNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles branch created notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(BranchCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Branch created: {BranchId} - {Name}", notification.Id, notification.Name);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("branch.created", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles branch updated notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(BranchUpdatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Branch updated: {BranchId} - {Name}", notification.Id, notification.Name);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("branch.updated", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles branch activated notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(BranchActivatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Branch activated: {BranchId} - {Name}", notification.Id, notification.Name);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("branch.activated", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles branch deactivated notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(BranchDeactivatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Branch deactivated: {BranchId} - {Name}", notification.Id, notification.Name);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("branch.deactivated", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles branch deleted notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(BranchDeletedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Branch deleted: {BranchId} - {Name}", notification.Id, notification.Name);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("branch.deleted", notification);

        return Task.CompletedTask;
    }
}
