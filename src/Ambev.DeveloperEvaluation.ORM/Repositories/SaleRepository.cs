using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implementation of ISaleRepository using Entity Framework Core.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of SaleRepository.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <inheritdoc />
        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<(List<Sale> Sales, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Sales.AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var sales = await query
                .Include(s => s.Items)
                .OrderByDescending(s => s.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (sales, totalCount);
        }

        /// <inheritdoc />
        public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Entry(sale).State = EntityState.Modified;

            // Handle collection updates
            var existingItems = await _context.Set<SaleItem>()
                .Where(i => EF.Property<Guid>(i, "SaleId") == sale.Id)
                .ToListAsync(cancellationToken);

            foreach (var existingItem in existingItems)
            {
                if (!sale.Items.Any(i => i.Id == existingItem.Id))
                {
                    _context.Set<SaleItem>().Remove(existingItem);
                }
            }

            foreach (var item in sale.Items)
            {
                var existingItem = existingItems.FirstOrDefault(i => i.Id == item.Id);
                if (existingItem == null)
                {
                    _context.Set<SaleItem>().Add(item);
                }
                else
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}