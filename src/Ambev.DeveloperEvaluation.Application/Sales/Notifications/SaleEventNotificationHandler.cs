using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

/// <summary>
/// Handler for all sale-related events.
/// </summary>
public class SaleEventNotificationHandler :
    INotificationHandler<SaleCreatedNotification>,
    INotificationHandler<SaleModifiedNotification>,
    INotificationHandler<SaleCancelledNotification>,
    INotificationHandler<SaleDeletedNotification>,
    INotificationHandler<ItemCancelledNotification>
{
    private readonly ILogger<SaleEventNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleEventNotificationHandler.
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public SaleEventNotificationHandler(ILogger<SaleEventNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles sale created notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale created: {SaleId}", notification.SaleId);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("sale.created", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles sale modified notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleModifiedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale modified: {SaleId}", notification.SaleId);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("sale.modified", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles sale cancelled notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale cancelled: {SaleId}", notification.SaleId);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("sale.cancelled", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles sale deleted notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleDeletedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale deleted: {SaleId}", notification.SaleId);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("sale.deleted", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles item cancelled notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ItemCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Item cancelled in sale {SaleId}: Product {ProductId}",
            notification.SaleId, notification.ProductId);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("sale.item.cancelled", notification);

        return Task.CompletedTask;
    }
}
