using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="mediator">The mediator for publishing notifications</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        IMediator mediator,
        ILogger<CreateSaleHandler> logger)
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
    /// Handles the CreateSaleCommand request.
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new sale with number: {Number}", command.Number);

        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new Ambev.DeveloperEvaluation.Domain.Exceptions.ValidationException(validationResult.Errors);

        // Validate Customer exists
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
        if (customer == null)
            throw new NotFoundException($"Customer with ID {command.CustomerId} not found.");

        // Validate Branch exists
        var branch = await _branchRepository.GetByIdAsync(command.BranchId);
        if (branch == null)
            throw new NotFoundException($"Branch with ID {command.BranchId} not found.");

        // Validate all Products exist
        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new NotFoundException($"Product with ID {item.ProductId} not found.");

            // Validate stock quantity
            if (product.StockQuantity < item.Quantity)
                throw new DomainException($"Insufficient stock for product '{product.Title}'. Available: {product.StockQuantity}, Requested: {item.Quantity}");
        }

        var sale = new Sale(
            command.Number,
            command.Date,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );

        var uniqueItems = command.Items
            .GroupBy(i => i.ProductId)
            .Select(g => g.First())
            .ToList();

        try
        {
            foreach (var item in uniqueItems)
            {
                sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
            }

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            foreach (var item in uniqueItems)
            {
                await _productRepository.UpdateStockAsync(
                    item.ProductId,
                    await GetUpdatedStockQuantity(item.ProductId, item.Quantity, cancellationToken)
                );
            }

            _logger.LogInformation("Sale created successfully with ID: {SaleId}", createdSale.Id);
            await _mediator.Publish(new SaleCreatedNotification { SaleId = createdSale.Id }, cancellationToken);

            return _mapper.Map<CreateSaleResult>(createdSale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sale {Number}: {ErrorMessage}", command.Number, ex.Message);
            throw;
        }
    }

    private async Task<int> GetUpdatedStockQuantity(Guid productId, int quantitySold, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product.StockQuantity - quantitySold;
    }
}