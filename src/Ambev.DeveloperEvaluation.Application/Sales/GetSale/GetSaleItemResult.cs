namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Sale item details.
    /// </summary>
    public class GetSaleItemResult
    {
        /// <summary>
        /// Unique identifier of the item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied to the item.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total price after discount.
        /// </summary>
        public decimal Total { get; set; }
    }
}
