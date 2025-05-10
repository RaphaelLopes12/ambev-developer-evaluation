using System.Linq.Dynamic.Core;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of Product repository using PostgreSQL
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the product repository
    /// </summary>
    /// <param name="context">PostgreSQL context</param>
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all products with pagination and sorting
    /// </summary>
    /// <param name="page">Current page</param>
    /// <param name="size">Page size</param>
    /// <param name="order">Ordering expression (e.g. "price desc, title asc")</param>
    /// <returns>Paginated list of products</returns>
    public async Task<(IEnumerable<Product> Data, int TotalItems, int CurrentPage, int TotalPages)> GetAllAsync(int page = 1, int size = 10, string order = null)
    {
        IQueryable<Product> query = _context.Products;

        if (!string.IsNullOrEmpty(order))
        {
            query = query.OrderBy(order);
        }
        else
        {
            query = query.OrderBy(p => p.Id);
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        // Apply pagination
        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (products, totalItems, page, totalPages);
    }

    /// <summary>
    /// Gets a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product found or null</returns>
    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    /// <summary>
    /// Gets all product categories
    /// </summary>
    /// <returns>List of categories</returns>
    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync();
    }

    /// <summary>
    /// Gets products by category with pagination and sorting
    /// </summary>
    /// <param name="category">Category name</param>
    /// <param name="page">Current page</param>
    /// <param name="size">Page size</param>
    /// <param name="order">Ordering expression (e.g. "price desc, title asc")</param>
    /// <returns>Paginated list of products in the category</returns>
    public async Task<(IEnumerable<Product> Data, int TotalItems, int CurrentPage, int TotalPages)> GetByCategoryAsync(string category, int page = 1, int size = 10, string order = null)
    {
        IQueryable<Product> query = _context.Products
            .Where(p => p.Category == category);


        if (!string.IsNullOrEmpty(order))
        {
            query = query.OrderBy(order);
        }
        else
        {
            query = query.OrderBy(p => p.Id);
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (products, totalItems, page, totalPages);
    }

    /// <summary>
    /// Adds a new product
    /// </summary>
    /// <param name="product">Product data</param>
    /// <returns>Created product</returns>
    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="title">Product title</param>
    /// <param name="price">Product price</param>
    /// <param name="description">Product description</param>
    /// <param name="category">Product category</param>
    /// <param name="image">Product image URL</param>
    /// <returns>True if updated successfully</returns>
    public async Task<bool> UpdateDetailsAsync(Guid id, string title, decimal price, string description, string category, string image)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        product.UpdateDetails(title, price, description, category, image);

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Updates the stock quantity of a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="quantity">New stock quantity</param>
    /// <returns>True if updated successfully</returns>
    public async Task<bool> UpdateStockAsync(Guid id, int quantity)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        product.UpdateStock(quantity);

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Adds a rating to a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="rating">Rating value (0-5)</param>
    /// <returns>True if updated successfully</returns>
    public async Task<bool> AddRatingAsync(Guid id, decimal rating)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        product.AddRating(rating);

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>True if deleted successfully</returns>
    public async Task<bool> RemoveAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}