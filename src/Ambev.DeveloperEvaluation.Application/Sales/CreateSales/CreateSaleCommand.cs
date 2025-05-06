using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    /// <summary>
    /// Command to create a new sale.
    /// </summary>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        /// <summary>
        /// Business identifier for the sale.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Date when the sale was made.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Customer ID (external).
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Customer name (denormalized).
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Branch ID (external).
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Branch name (denormalized).
        /// </summary>
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// List of items in the sale.
        /// </summary>
        public List<CreateSaleItemDto> Items { get; set; } = new();

        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
