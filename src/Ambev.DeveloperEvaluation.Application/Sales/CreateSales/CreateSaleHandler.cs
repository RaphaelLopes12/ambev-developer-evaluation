using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="mediator">The mediator for publishing notifications</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IMediator mediator,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
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
            throw new ValidationException(validationResult.Errors);

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
}