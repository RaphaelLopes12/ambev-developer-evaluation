using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

/// <summary>
/// Notification for sale deletion events.
/// </summary>
public class SaleDeletedNotification : INotification
{
    /// <summary>
    /// The identifier of the deleted sale.
    /// </summary>
    public Guid SaleId { get; set; }
}
