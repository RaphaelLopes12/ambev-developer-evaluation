using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Notifications;

/// <summary>
/// Notification for product creation events.
/// </summary>
public class ProductCreatedNotification : INotification
{
    /// <summary>
    /// The identifier of the created product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the created product.
    /// </summary>
    public string Title { get; set; }
}
