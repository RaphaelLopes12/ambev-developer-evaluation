using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Sale entity operations.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Creates a new sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale.</returns>
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a sale by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of sales.
        /// </summary>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of sales for the requested page.</returns>
        Task<(List<Sale> Sales, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale.</returns>
        Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a sale from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the sale was deleted, false if not found.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
