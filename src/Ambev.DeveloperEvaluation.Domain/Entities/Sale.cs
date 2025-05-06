using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale with customer, branch, items and discount rules.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Unique identifier of the sale.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Number of the sale (business identifier).
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Date the sale occurred.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Customer ID (external reference).
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Customer name (denormalized).
        /// </summary>
        public string CustomerName { get; private set; }

        /// <summary>
        /// Branch ID (external reference).
        /// </summary>
        public Guid BranchId { get; private set; }

        /// <summary>
        /// Branch name (denormalized).
        /// </summary>
        public string BranchName { get; private set; }

        /// <summary>
        /// List of items sold.
        /// </summary>
        public List<SaleItem> Items { get; private set; } = new();

        /// <summary>
        /// Total amount of the sale (sum of item totals).
        /// </summary>
        public decimal TotalAmount => Items.Sum(i => i.Total);

        /// <summary>
        /// Current status of the sale.
        /// </summary>
        public SaleStatus Status { get; private set; }

        /// <summary>
        /// Timestamp of creation.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Timestamp of last update.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Required for EF.
        /// </summary>
        public Sale() { }

        public Sale(string number, DateTime date, Guid customerId, string customerName, Guid branchId, string branchName)
        {
            Id = Guid.NewGuid();
            Number = number;
            Date = date;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            Status = SaleStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds a product item to the sale.
        /// </summary>
        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            var item = new SaleItem(productId, productName, quantity, unitPrice);
            Items.Add(item);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancels the sale.
        /// </summary>
        public void Cancel()
        {
            Status = SaleStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Validates the current sale state.
        /// </summary>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}