using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

/// <summary>
/// Notification for sale creation events.
/// </summary>
public class SaleCreatedNotification : INotification
{
    /// <summary>
    /// The identifier of the created sale.
    /// </summary>
    public Guid SaleId { get; set; }
}
