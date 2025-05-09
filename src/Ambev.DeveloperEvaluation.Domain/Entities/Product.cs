using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product available for sale.
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Title/name of the product.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Detailed description of the product.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Category of the product.
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// URL of the product image.
    /// </summary>
    public string Image { get; private set; }

    /// <summary>
    /// Quantity in stock.
    /// </summary>
    public int StockQuantity { get; private set; }

    /// <summary>
    /// Average rating of the product (0-5).
    /// </summary>
    public decimal RatingRate { get; private set; }

    /// <summary>
    /// Number of ratings received.
    /// </summary>
    public int RatingCount { get; private set; }

    /// <summary>
    /// Creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Last update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Sale items related to this product.
    /// </summary>
    public virtual ICollection<SaleItem> SaleItems { get; private set; }

    /// <summary>
    /// Required for EF.
    /// </summary>
    public Product() { }

    /// <summary>
    /// Creates a new product with the specified details.
    /// </summary>
    public Product(string title, decimal price, string description, string category, string image, int stockQuantity, decimal ratingRate = 0, int ratingCount = 0)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        StockQuantity = stockQuantity;
        RatingRate = ratingRate;
        RatingCount = ratingCount;
        CreatedAt = DateTime.UtcNow;
        SaleItems = new List<SaleItem>();
    }

    /// <summary>
    /// Updates the product details.
    /// </summary>
    public void UpdateDetails(string title, decimal price, string description, string category, string image)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the stock quantity.
    /// </summary>
    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(quantity));

        StockQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Decreases the stock quantity.
    /// </summary>
    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (StockQuantity < quantity)
            throw new InvalidOperationException("Not enough items in stock.");

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Increases the stock quantity.
    /// </summary>
    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new rating to the product.
    /// </summary>
    public void AddRating(decimal rate)
    {
        if (rate < 0 || rate > 5)
            throw new ArgumentException("Rating must be between 0 and 5.", nameof(rate));

        // Calculate new average rating
        decimal totalRating = (RatingRate * RatingCount) + rate;
        RatingCount++;
        RatingRate = totalRating / RatingCount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the current product state.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new ProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }
}

/// <summary>
/// Validator for Product entity.
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100).WithMessage("Category cannot exceed 100 characters.");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

        RuleFor(p => p.RatingRate)
            .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");
    }
}