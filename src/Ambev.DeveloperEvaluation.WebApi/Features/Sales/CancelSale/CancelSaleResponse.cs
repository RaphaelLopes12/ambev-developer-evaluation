namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    /// <summary>
    /// Response for cancelling a sale.
    /// </summary>
    public class CancelSaleResponse
    {
        /// <summary>
        /// Indicates whether the cancellation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The identifier of the cancelled sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// A message describing the result of the operation.
        /// </summary>
        public string Message { get; set; }
    }
}
