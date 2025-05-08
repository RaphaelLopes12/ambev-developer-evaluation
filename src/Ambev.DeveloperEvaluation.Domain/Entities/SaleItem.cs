using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents an individual product sold within a sale.
    /// </summary>
    public class SaleItem
    {
        /// <summary>
        /// Unique identifier for the sale item.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Product ID (external reference).
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Product name (denormalized).
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// Quantity of the product sold.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Price per unit.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Discount applied based on quantity tiers.
        /// </summary>
        public decimal Discount { get; private set; }

        /// <summary>
        /// Indicates if this item has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Total amount for this item (after discount).
        /// </summary>
        public decimal Total => IsCancelled ? 0 : (UnitPrice * Quantity) - Discount;

        /// <summary>
        /// Required for EF.
        /// </summary>
        public SaleItem() { }

        /// <summary>
        /// Creates a new sale item.
        /// </summary>
        public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            IsCancelled = false;
            Discount = CalculateDiscount(quantity, unitPrice);
        }

        /// <summary>
        /// Updates the quantity and recalculates the discount.
        /// </summary>
        public void UpdateQuantity(int quantity, decimal discount)
        {
            Quantity = quantity;
            Discount = discount;
        }

        /// <summary>
        /// Cancels this item.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
        }

        /// <summary>
        /// Calculates discount based on business rules.
        /// </summary>
        private decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 items of the same product");

            if (quantity >= 10)
                return quantity * unitPrice * 0.20m;

            if (quantity >= 4)
                return quantity * unitPrice * 0.10m;

            return 0;
        }

        /// <summary>
        /// Validates item business rules.
        /// </summary>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleItemValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}