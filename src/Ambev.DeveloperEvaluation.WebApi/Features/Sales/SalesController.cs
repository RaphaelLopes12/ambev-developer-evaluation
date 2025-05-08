using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateSaleRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(result)
        });
    }

    /// <summary>
    /// Gets a sale by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleQuery { Id = id }, cancellationToken);

        if (result == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Sale not found"
            });

        return Ok(new ApiResponseWithData<GetSaleResponse>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = _mapper.Map<GetSaleResponse>(result)
        });
    }

    /// <summary>
    /// Gets all sales (paged)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSalesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesRequest request, CancellationToken cancellationToken = default)
    {
        var query = _mapper.Map<GetSalesQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponseWithData<GetSalesResponse>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = _mapper.Map<GetSalesResponse>(result)
        });
    }

    /// <summary>
    /// Updates a sale
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateSaleRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = result.Message
                });

            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = result.Message
            });
        }

        return Ok(new ApiResponseWithData<UpdateSaleResponse>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = _mapper.Map<UpdateSaleResponse>(result)
        });
    }

    /// <summary>
    /// Cancels a sale
    /// </summary>
    [HttpPatch("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelSaleCommand { Id = id }, cancellationToken);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = result.Message
                });

            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = result.Message
            });
        }

        return Ok(new ApiResponseWithData<CancelSaleResponse>
        {
            Success = true,
            Message = "Sale cancelled successfully",
            Data = _mapper.Map<CancelSaleResponse>(result)
        });
    }

    /// <summary>
    /// Cancels an item of a sale
    /// </summary>
    [HttpPatch("{saleId}/items/{productId}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSaleItem([FromRoute] Guid saleId, [FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var command = new CancelSaleItemCommand
        {
            SaleId = saleId,
            ProductId = productId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = result.Message
                });

            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = result.Message
            });
        }

        return Ok(new ApiResponseWithData<CancelSaleItemResponse>
        {
            Success = true,
            Message = "Sale item cancelled successfully",
            Data = _mapper.Map<CancelSaleItemResponse>(result)
        });
    }

    /// <summary>
    /// Deletes a sale
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new DeleteSaleCommand { Id = id }, cancellationToken);

        if (!success)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Sale not found"
            });

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Sale deleted successfully"
        });
    }
}