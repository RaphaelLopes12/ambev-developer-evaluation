using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.MongoDB;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementação do repositório de Clientes usando MongoDB
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly MongoDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância do repositório de clientes
    /// </summary>
    /// <param name="context">Contexto do MongoDB</param>
    public CustomerRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém todos os clientes
    /// </summary>
    /// <returns>Lista de clientes</returns>
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Obtém um cliente pelo seu ID
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>Cliente encontrado ou null</returns>
    public async Task<Customer> GetByIdAsync(string id)
    {
        return await _context.Customers.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Adiciona um novo cliente
    /// </summary>
    /// <param name="customer">Dados do cliente</param>
    /// <returns>Cliente criado</returns>
    public async Task<Customer> AddAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        await _context.Customers.InsertOneAsync(customer);
        return customer;
    }

    /// <summary>
    /// Atualiza os dados de um cliente existente
    /// </summary>
    /// <param name="customer">Dados atualizados do cliente</param>
    /// <returns>True se atualizado com sucesso</returns>
    public async Task<bool> UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var result = await _context.Customers.ReplaceOneAsync(c => c.Id == customer.Id, customer);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    /// <summary>
    /// Remove um cliente pelo seu ID
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>True se removido com sucesso</returns>
    public async Task<bool> RemoveAsync(string id)
    {
        var result = await _context.Customers.DeleteOneAsync(c => c.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
