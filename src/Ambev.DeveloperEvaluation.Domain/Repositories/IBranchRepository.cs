using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Interface for Branch repository operations
/// </summary>
public interface IBranchRepository
{
    /// <summary>
    /// Gets all branches
    /// </summary>
    /// <returns>List of branches</returns>
    Task<IEnumerable<Branch>> GetAllAsync();

    /// <summary>
    /// Gets a branch by its ID
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>Branch found or null</returns>
    Task<Branch> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new branch
    /// </summary>
    /// <param name="branch">Branch data</param>
    /// <returns>Created branch</returns>
    Task<Branch> AddAsync(Branch branch);

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
    Task<bool> UpdateDetailsAsync(int id, string name, string address, string city, string state, string zipCode, string phone, string email);

    /// <summary>
    /// Activates a branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>True if activated successfully</returns>
    Task<bool> ActivateAsync(int id);

    /// <summary>
    /// Deactivates a branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>True if deactivated successfully</returns>
    Task<bool> DeactivateAsync(int id);

    /// <summary>
    /// Deletes a branch by its ID
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>True if deleted successfully</returns>
    Task<bool> RemoveAsync(int id);
}