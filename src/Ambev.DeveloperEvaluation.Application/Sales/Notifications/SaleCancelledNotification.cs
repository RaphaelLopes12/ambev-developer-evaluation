using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

/// <summary>
/// Notification for sale cancellation events.
/// </summary>
public class SaleCancelledNotification : INotification
{
    /// <summary>
    /// The identifier of the cancelled sale.
    /// </summary>
    public Guid SaleId { get; set; }
}
