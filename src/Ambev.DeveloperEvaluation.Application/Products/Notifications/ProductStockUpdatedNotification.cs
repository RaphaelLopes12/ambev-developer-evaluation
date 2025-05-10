using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Notifications;

/// <summary>
/// Notification for product stock update events.
/// </summary>
public class ProductStockUpdatedNotification : INotification
{
    /// <summary>
    /// The identifier of the product with updated stock.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the product.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The previous stock quantity.
    /// </summary>
    public int PreviousQuantity { get; set; }

    /// <summary>
    /// The new stock quantity.
    /// </summary>
    public int NewQuantity { get; set; }
}
