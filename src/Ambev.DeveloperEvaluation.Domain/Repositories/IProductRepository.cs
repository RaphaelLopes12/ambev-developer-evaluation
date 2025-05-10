using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Interface for Product repository operations
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets all products with pagination and sorting
    /// </summary>
    /// <param name="page">Current page</param>
    /// <param name="size">Page size</param>
    /// <param name="order">Ordering expression (e.g. "price desc, title asc")</param>
    /// <returns>Paginated list of products</returns>
    Task<(IEnumerable<Product> Data, int TotalItems, int CurrentPage, int TotalPages)> GetAllAsync(int page = 1, int size = 10, string order = null);

    /// <summary>
    /// Gets a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product found or null</returns>
    Task<Product> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all product categories
    /// </summary>
    /// <returns>List of categories</returns>
    Task<IEnumerable<string>> GetCategoriesAsync();

    /// <summary>
    /// Gets products by category with pagination and sorting
    /// </summary>
    /// <param name="category">Category name</param>
    /// <param name="page">Current page</param>
    /// <param name="size">Page size</param>
    /// <param name="order">Ordering expression (e.g. "price desc, title asc")</param>
    /// <returns>Paginated list of products in the category</returns>
    Task<(IEnumerable<Product> Data, int TotalItems, int CurrentPage, int TotalPages)> GetByCategoryAsync(string category, int page = 1, int size = 10, string order = null);

    /// <summary>
    /// Adds a new product
    /// </summary>
    /// <param name="product">Product data</param>
    /// <returns>Created product</returns>
    Task<Product> AddAsync(Product product);

    /// <summary>
    /// Updates the details of an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="title">Product title</param>
    /// <param name="price">Product price</param>
    /// <param name="description">Product description</param>
    /// <param name="category">Product category</param>
    /// <param name="image">Product image URL</param>
    /// <returns>True if updated successfully</returns>
    Task<bool> UpdateDetailsAsync(Guid id, string title, decimal price, string description, string category, string image);

    /// <summary>
    /// Updates the stock quantity of a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="quantity">New stock quantity</param>
    /// <returns>True if updated successfully</returns>
    Task<bool> UpdateStockAsync(Guid id, int quantity);

    /// <summary>
    /// Adds a rating to a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="rating">Rating value (0-5)</param>
    /// <returns>True if updated successfully</returns>
    Task<bool> AddRatingAsync(Guid id, decimal rating);

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>True if deleted successfully</returns>
    Task<bool> RemoveAsync(Guid id);
}