using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for updating an existing sale.
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler.
    /// </summary>
    /// <param name="saleRepository">Sale repository</param>
    /// <param name="customerRepository">Customer repository</param>
    /// <param name="branchRepository">Branch repository</param>
    /// <param name="productRepository">Product repository</param>
    /// <param name="mapper">Automapper instance</param>
    /// <param name="mediator">Mediator instance for publishing events</param>
    /// <param name="logger">Logger instance</param>
    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        IMediator mediator,
        ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
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

        if (sale.Status.ToString() == "Cancelled")
        {
            _logger.LogWarning("Attempt to update cancelled sale with ID: {SaleId}", request.Id);
            return new UpdateSaleResult
            {
                Success = false,
                Id = request.Id,
                Message = "Cannot update a cancelled sale"
            };
        }

        try
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found with ID: {CustomerId} when updating sale: {SaleId}",
                    request.CustomerId, request.Id);
                return new UpdateSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = $"Customer with ID {request.CustomerId} not found"
                };
            }

            var branch = await _branchRepository.GetByIdAsync(request.BranchId);
            if (branch == null)
            {
                _logger.LogWarning("Branch not found with ID: {BranchId} when updating sale: {SaleId}",
                    request.BranchId, request.Id);
                return new UpdateSaleResult
                {
                    Success = false,
                    Id = request.Id,
                    Message = $"Branch with ID {request.BranchId} not found"
                };
            }

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId} when updating sale: {SaleId}",
                        item.ProductId, request.Id);
                    return new UpdateSaleResult
                    {
                        Success = false,
                        Id = request.Id,
                        Message = $"Product with ID {item.ProductId} not found"
                    };
                }

                if (item.Quantity > 20)
                {
                    _logger.LogWarning("Quantity exceeds maximum allowed: {Quantity} for product: {ProductId}",
                        item.Quantity, item.ProductId);
                    return new UpdateSaleResult
                    {
                        Success = false,
                        Id = request.Id,
                        Message = $"Cannot add more than 20 units of the same product ({item.ProductName})"
                    };
                }

                if (item.Quantity <= 0)
                {
                    _logger.LogWarning("Invalid quantity: {Quantity} for product: {ProductId}",
                        item.Quantity, item.ProductId);
                    return new UpdateSaleResult
                    {
                        Success = false,
                        Id = request.Id,
                        Message = $"Quantity must be greater than zero for product {item.ProductName}"
                    };
                }
            }

            sale.UpdateDetails(request.Date, request.CustomerId, request.CustomerName, request.BranchId, request.BranchName);
            _logger.LogDebug("Updated sale details for ID: {SaleId}", request.Id);

            var existingProductToItemMapping = sale.Items
                .Where(item => !item.IsCancelled)
                .ToDictionary(item => item.ProductId, item => item);

            var currentProductIds = existingProductToItemMapping.Keys.ToHashSet();
            var requestProductIds = request.Items.Select(i => i.ProductId).ToHashSet();
            var productIdsToRemove = currentProductIds.Except(requestProductIds).ToList();

            foreach (var productId in productIdsToRemove)
            {
                if (existingProductToItemMapping.TryGetValue(productId, out var existingItem))
                {
                    _logger.LogDebug("Removing item with ProductId: {ProductId} from sale: {SaleId}",
                        productId, request.Id);

                    var product = await _productRepository.GetByIdAsync(productId);
                    if (product != null)
                    {
                        await _productRepository.UpdateStockAsync(
                            productId,
                            product.StockQuantity + existingItem.Quantity
                        );

                        _logger.LogDebug("Returned {Quantity} units to stock for product {ProductId}",
                            existingItem.Quantity, productId);
                    }

                    sale.RemoveItem(productId);
                }
            }

            foreach (var itemDto in request.Items)
            {
                if (existingProductToItemMapping.TryGetValue(itemDto.ProductId, out var existingItem))
                {
                    int quantityDifference = itemDto.Quantity - existingItem.Quantity;

                    if (quantityDifference != 0)
                    {
                        var product = await _productRepository.GetByIdAsync(itemDto.ProductId);

                        if (quantityDifference > 0)
                        {
                            if (product.StockQuantity < quantityDifference)
                            {
                                _logger.LogWarning(
                                    "Insufficient stock for product {ProductId}. Available: {Available}, Additional Requested: {Requested}",
                                    itemDto.ProductId, product.StockQuantity, quantityDifference);

                                return new UpdateSaleResult
                                {
                                    Success = false,
                                    Id = request.Id,
                                    Message = $"Insufficient stock for product {itemDto.ProductName}. Available: {product.StockQuantity}, Additional requested: {quantityDifference}"
                                };
                            }

                            await _productRepository.UpdateStockAsync(
                                itemDto.ProductId,
                                product.StockQuantity - quantityDifference
                            );

                            _logger.LogDebug("Decreased stock by {Quantity} for product {ProductId}",
                                quantityDifference, itemDto.ProductId);
                        }
                        else if (quantityDifference < 0)
                        {
                            await _productRepository.UpdateStockAsync(
                                itemDto.ProductId,
                                product.StockQuantity + Math.Abs(quantityDifference)
                            );

                            _logger.LogDebug("Increased stock by {Quantity} for product {ProductId}",
                                Math.Abs(quantityDifference), itemDto.ProductId);
                        }
                    }

                    sale.UpdateItem(itemDto.ProductId, itemDto.Quantity);
                    _logger.LogDebug("Updated existing item with ProductId: {ProductId} in sale: {SaleId}",
                        itemDto.ProductId, request.Id);
                }
                else
                {
                    var product = await _productRepository.GetByIdAsync(itemDto.ProductId);

                    if (product.StockQuantity < itemDto.Quantity)
                    {
                        _logger.LogWarning(
                            "Insufficient stock for new product {ProductId}. Available: {Available}, Requested: {Requested}",
                            itemDto.ProductId, product.StockQuantity, itemDto.Quantity);

                        return new UpdateSaleResult
                        {
                            Success = false,
                            Id = request.Id,
                            Message = $"Insufficient stock for product {itemDto.ProductName}. Available: {product.StockQuantity}, Requested: {itemDto.Quantity}"
                        };
                    }

                    await _productRepository.UpdateStockAsync(
                        itemDto.ProductId,
                        product.StockQuantity - itemDto.Quantity
                    );

                    _logger.LogDebug("Decreased stock by {Quantity} for new product {ProductId}",
                        itemDto.Quantity, itemDto.ProductId);

                    sale.AddItem(itemDto.ProductId, itemDto.ProductName, itemDto.Quantity, itemDto.UnitPrice);
                    _logger.LogDebug("Added new item with ProductId: {ProductId} to sale: {SaleId}",
                        itemDto.ProductId, request.Id);
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
        catch (DomainException dx)
        {
            _logger.LogWarning(dx, "Domain exception updating sale with ID {SaleId}: {ErrorMessage}",
                request.Id, dx.Message);

            return new UpdateSaleResult
            {
                Success = false,
                Id = request.Id,
                Message = dx.Message
            };
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