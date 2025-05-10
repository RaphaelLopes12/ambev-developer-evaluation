namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Defines the possible statuses of a sale.
/// </summary>
public enum SaleStatus
{
    /// <summary>
    /// Sale is active and valid.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Sale is cancelled.
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// Sale is completed and finalized.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Sale is pending approval or payment.
    /// </summary>
    Pending = 4
}
