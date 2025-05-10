using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Notifications;

/// <summary>
/// Handler for all product-related events.
/// </summary>
public class ProductEventNotificationHandler :
    INotificationHandler<ProductCreatedNotification>,
    INotificationHandler<ProductUpdatedNotification>,
    INotificationHandler<ProductStockUpdatedNotification>,
    INotificationHandler<ProductDeletedNotification>
{
    private readonly ILogger<ProductEventNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ProductEventNotificationHandler.
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public ProductEventNotificationHandler(ILogger<ProductEventNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles product created notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product created: {ProductId} - {Title}", notification.Id, notification.Title);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("product.created", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles product updated notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ProductUpdatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product updated: {ProductId} - {Title}", notification.Id, notification.Title);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("product.updated", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles product stock updated notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ProductStockUpdatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product stock updated: {ProductId} - {Title}, Previous: {Previous}, New: {New}",
            notification.Id, notification.Title, notification.PreviousQuantity, notification.NewQuantity);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("product.stock.updated", notification);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles product deleted notifications.
    /// </summary>
    /// <param name="notification">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ProductDeletedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product deleted: {ProductId} - {Title}", notification.Id, notification.Title);

        // Here you would implement the actual event publishing to a message broker
        // For example:
        // await _messageBroker.PublishAsync("product.deleted", notification);

        return Task.CompletedTask;
    }
}
