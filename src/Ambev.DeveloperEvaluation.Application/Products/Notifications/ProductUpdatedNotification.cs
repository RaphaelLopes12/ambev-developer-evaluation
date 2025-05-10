using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Notifications;

/// <summary>
/// Notification for product update events.
/// </summary>
public class ProductUpdatedNotification : INotification
{
    /// <summary>
    /// The identifier of the updated product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the updated product.
    /// </summary>
    public string Title { get; set; }
}
