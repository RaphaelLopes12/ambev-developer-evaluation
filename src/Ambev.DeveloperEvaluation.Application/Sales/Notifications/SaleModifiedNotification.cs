using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

/// <summary>
/// Notification for sale modification events.
/// </summary>
public class SaleModifiedNotification : INotification
{
    /// <summary>
    /// The identifier of the modified sale.
    /// </summary>
    public Guid SaleId { get; set; }
}
