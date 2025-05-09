using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Interface de repositório para operações com Clientes
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Obtém todos os clientes
    /// </summary>
    /// <returns>Lista de clientes</returns>
    Task<IEnumerable<Customer>> GetAllAsync();

    /// <summary>
    /// Obtém um cliente pelo seu ID
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>Cliente encontrado ou null</returns>
    Task<Customer> GetByIdAsync(string id);

    /// <summary>
    /// Adiciona um novo cliente
    /// </summary>
    /// <param name="customer">Dados do cliente</param>
    /// <returns>Cliente criado</returns>
    Task<Customer> AddAsync(Customer customer);

    /// <summary>
    /// Atualiza os dados de um cliente existente
    /// </summary>
    /// <param name="customer">Dados atualizados do cliente</param>
    /// <returns>True se atualizado com sucesso</returns>
    Task<bool> UpdateAsync(Customer customer);

    /// <summary>
    /// Remove um cliente pelo seu ID
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>True se removido com sucesso</returns>
    Task<bool> RemoveAsync(string id);
}
