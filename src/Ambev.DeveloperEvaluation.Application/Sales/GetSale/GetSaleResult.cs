namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Result object returning sale details.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// Unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Business identifier of the sale.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Date when the sale was made.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Customer identifier.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Branch identifier.
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Branch name.
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Status of the sale.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Items in the sale.
        /// </summary>
        public List<GetSaleItemResult> Items { get; set; } = new();

        /// <summary>
        /// Date and time when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the sale was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
