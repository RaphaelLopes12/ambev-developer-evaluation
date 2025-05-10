using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Notifications;

/// <summary>
/// Notification for product deletion events.
/// </summary>
public class ProductDeletedNotification : INotification
{
    /// <summary>
    /// The identifier of the deleted product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the deleted product.
    /// </summary>
    public string Title { get; set; }
}
