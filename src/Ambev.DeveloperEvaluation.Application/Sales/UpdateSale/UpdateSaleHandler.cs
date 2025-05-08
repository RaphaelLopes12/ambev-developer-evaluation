using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for updating an existing sale.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateSaleHandler> _logger;

        /// <summary>
        /// Initializes a new instance of UpdateSaleHandler.
        /// </summary>
        /// <param name="saleRepository">Sale repository</param>
        /// <param name="mapper">Automapper instance</param>
        /// <param name="mediator">Mediator instance for publishing events</param>
        /// <param name="logger">Logger instance</param>
        public UpdateSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            IMediator mediator,
            ILogger<UpdateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Handles the command to update an existing sale.
        /// </summary>
        /// <param name="request">The command parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the update operation</returns>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating sale with ID: {SaleId}", request.Id);

            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale not found with ID: {SaleId}", request.Id);
                return new UpdateSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = "Sale not found"
                };
            }

            try
            {
                sale.UpdateDetails(request.Date, request.CustomerId, request.CustomerName, request.BranchId, request.BranchName);
                _logger.LogDebug("Updated sale details for ID: {SaleId}", request.Id);

                var existingProductToItemMapping = sale.Items
                    .ToDictionary(item => item.ProductId, item => item.Id);

                var currentProductIds = sale.Items.Select(i => i.ProductId).ToHashSet();
                var requestProductIds = request.Items.Select(i => i.ProductId).ToHashSet();

                var productIdsToRemove = currentProductIds.Except(requestProductIds).ToList();
                foreach (var productId in productIdsToRemove)
                {
                    _logger.LogDebug("Removing item with ProductId: {ProductId} from sale: {SaleId}",
                        productId, request.Id);
                    sale.RemoveItem(productId);
                }

                foreach (var itemDto in request.Items)
                {
                    if (existingProductToItemMapping.TryGetValue(itemDto.ProductId, out var existingItemId))
                    {
                        _logger.LogDebug("Updating existing item with ProductId: {ProductId} in sale: {SaleId}",
                            itemDto.ProductId, request.Id);
                        sale.UpdateItem(itemDto.ProductId, itemDto.Quantity);
                    }
                    else
                    {
                        _logger.LogDebug("Adding new item with ProductId: {ProductId} to sale: {SaleId}",
                            itemDto.ProductId, request.Id);
                        sale.AddItem(itemDto.ProductId, itemDto.ProductName, itemDto.Quantity, itemDto.UnitPrice);
                    }
                }

                var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
                _logger.LogInformation("Sale updated successfully with ID: {SaleId}", sale.Id);

                await _mediator.Publish(new SaleModifiedNotification { SaleId = sale.Id }, cancellationToken);

                var result = _mapper.Map<UpdateSaleResult>(updatedSale);
                result.Success = true;
                result.Message = "Sale updated successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sale with ID {SaleId}: {ErrorMessage}",
                    request.Id, ex.Message);

                return new UpdateSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = $"Failed to update sale: {ex.Message}"
                };
            }
        }
    }
}