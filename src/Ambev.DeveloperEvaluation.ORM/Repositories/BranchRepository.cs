using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of Branch repository using PostgreSQL
/// </summary>
public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the branch repository
    /// </summary>
    /// <param name="context">PostgreSQL context</param>
    public BranchRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all branches
    /// </summary>
    /// <returns>List of branches</returns>
    public async Task<IEnumerable<Branch>> GetAllAsync()
    {
        return await _context.Branches.ToListAsync();
    }

    /// <summary>
    /// Gets a branch by its ID
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>Branch found or null</returns>
    public async Task<Branch> GetByIdAsync(int id)
    {
        return await _context.Branches.FindAsync(id);
    }

    /// <summary>
    /// Adds a new branch
    /// </summary>
    /// <param name="branch">Branch data</param>
    /// <returns>Created branch</returns>
    public async Task<Branch> AddAsync(Branch branch)
    {
        // Note: Branch entity now has private setters and CreatedAt is set in constructor,
        // so we don't need to manually set it here

        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    /// <summary>
    /// Updates the details of an existing branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <param name="name">Branch name</param>
    /// <param name="address">Branch address</param>
    /// <param name="city">Branch city</param>
    /// <param name="state">Branch state</param>
    /// <param name="zipCode">Branch zip code</param>
    /// <param name="phone">Branch phone</param>
    /// <param name="email">Branch email</param>
    /// <returns>True if updated successfully</returns>
    public async Task<bool> UpdateDetailsAsync(int id, string name, string address, string city, string state, string zipCode, string phone, string email)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            return false;

        // Use the entity's method instead of directly setting properties
        branch.UpdateDetails(name, address, city, state, zipCode, phone, email);

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Activates a branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>True if activated successfully</returns>
    public async Task<bool> ActivateAsync(int id)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            return false;

        // Use the entity's method
        branch.Activate();

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Deactivates a branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>True if deactivated successfully</returns>
    public async Task<bool> DeactivateAsync(int id)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            return false;

        // Use the entity's method
        branch.Deactivate();

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Deletes a branch by its ID
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>True if deleted successfully</returns>
    public async Task<bool> RemoveAsync(int id)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            return false;

        _context.Branches.Remove(branch);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}