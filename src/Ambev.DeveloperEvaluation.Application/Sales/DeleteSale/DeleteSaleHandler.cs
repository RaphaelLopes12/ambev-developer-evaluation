using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for deleting a sale.
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<DeleteSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">Sale repository</param>
    /// <param name="logger">Logger instance</param>
    public DeleteSaleHandler(ISaleRepository saleRepository, ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command for deleting a sale.
    /// </summary>
    /// <param name="request">The command parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false otherwise</returns>
    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting sale with ID: {SaleId}", request.Id);

        var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);

        if (success)
        {
            _logger.LogInformation("Sale with ID: {SaleId} deleted successfully", request.Id);
        }
        else
        {
            _logger.LogWarning("Sale with ID: {SaleId} not found for deletion", request.Id);
        }

        return success;
    }
}
