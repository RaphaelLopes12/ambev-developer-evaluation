namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Result of a sale cancellation.
    /// </summary>
    public class CancelSaleResult
    {
        /// <summary>
        /// Indicates whether the cancellation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The unique identifier of the cancelled sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Message describing the result of the operation.
        /// </summary>
        public string Message { get; set; }
    }
}
