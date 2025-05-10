using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

/// <summary>
/// Notification for item cancellation events.
/// </summary>
public class ItemCancelledNotification : INotification
{
    /// <summary>
    /// The identifier of the sale containing the cancelled item.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The identifier of the product that was cancelled.
    /// </summary>
    public Guid ProductId { get; set; }
}
