using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

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
    public string CustomerId { get; private set; }

    /// <summary>
    /// Customer name (denormalized).
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Branch ID (external reference).
    /// </summary>
    public int BranchId { get; private set; }

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
    public decimal TotalAmount => Items.Where(i => !i.IsCancelled).Sum(i => i.Total);

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

    /// <summary>
    /// Creates a new sale with the specified details.
    /// </summary>
    public Sale(string number, DateTime date, string customerId, string customerName, int branchId, string branchName)
    {
        Id = Guid.NewGuid();
        Number = number;
        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
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
    public void AddItem(int productId, string productName, int quantity, decimal unitPrice)
    {
        ValidateItemQuantity(quantity);

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        if (existingItem != null)
        {
            throw new ArgumentException($"Product {productId} already exists in this sale. Please update the existing item.");
        }

        var item = new SaleItem(productId, productName, quantity, unitPrice);
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates an existing item in the sale.
    /// </summary>
    public void UpdateItem(int productId, int quantity)
    {
        ValidateItemQuantity(quantity);

        var item = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Item with product ID {productId} not found in this sale or is cancelled.");
        }

        // Recalculate discount based on new quantity
        decimal discount = CalculateDiscount(quantity, item.UnitPrice);

        // Update item
        item.UpdateQuantity(quantity, discount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    public void RemoveItem(int productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
        {
            throw new ArgumentException($"Item with product ID {productId} not found in this sale");
        }

        Items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels a specific item in the sale.
    /// </summary>
    public void CancelItem(int productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Item with product ID {productId} not found in this sale or is already cancelled.");
        }

        item.Cancel();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("This sale is already cancelled.");
        }

        Status = SaleStatus.Cancelled;

        foreach (var item in Items.Where(i => !i.IsCancelled))
        {
            item.Cancel();
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the details of the sale.
    /// </summary>
    public void UpdateDetails(DateTime date, string customerId, string customerName, int branchId, string branchName)
    {
        Date = date;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
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

    /// <summary>
    /// Validates if the item quantity meets business rules.
    /// </summary>
    private void ValidateItemQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (quantity > 20)
            throw new ArgumentException("Cannot sell more than 20 identical items.", nameof(quantity));
    }

    /// <summary>
    /// Calculates discount based on business rules.
    /// </summary>
    private decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new ArgumentException("Cannot sell more than 20 items of the same product");

        if (quantity >= 10 && quantity <= 20)
            return quantity * unitPrice * 0.20m;

        if (quantity >= 4)
            return quantity * unitPrice * 0.10m;

        return 0;
    }
}