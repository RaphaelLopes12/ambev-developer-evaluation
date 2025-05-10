using Ambev.DeveloperEvaluation.Application.Products.AddRating;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProductById;
using Ambev.DeveloperEvaluation.Application.Products.GetProductCategories;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateStock;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.AddRating;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// Controller for managing product operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of ProductsController
    /// </summary>
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateProductRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateProductCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
        {
            Success = true,
            Message = "Product created successfully",
            Data = _mapper.Map<CreateProductResponse>(result)
        });
    }

    /// <summary>
    /// Gets a product by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id }, cancellationToken);

        if (result == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Product not found"
            });

        return Ok(new ApiResponseWithData<GetProductResponse>
        {
            Success = true,
            Message = "Product retrieved successfully",
            Data = _mapper.Map<GetProductResponse>(result)
        });
    }

    /// <summary>
    /// Gets all products with pagination and sorting
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await new GetProductsRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = _mapper.Map<GetProductsQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(_mapper.Map<GetProductsResponse>(result));
    }

    /// <summary>
    /// Gets all product categories
    /// </summary>
    [HttpGet("categories")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductCategoriesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetProductCategoriesQuery(), cancellationToken);

        return Ok(new ApiResponseWithData<GetProductCategoriesResponse>
        {
            Success = true,
            Message = "Categories retrieved successfully",
            Data = new GetProductCategoriesResponse
            {
                Categories = new List<string>(result)
            }
        });
    }

    /// <summary>
    /// Gets products by category with pagination and sorting
    /// </summary>
    [HttpGet("category/{category}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductsByCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByCategory(
        [FromRoute] string category,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string order = null,
        CancellationToken cancellationToken = default)
    {
        var request = new GetProductsByCategoryRequest
        {
            Category = category,
            Page = page,
            Size = size,
            Order = order
        };

        var validationResult = await new GetProductsByCategoryRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = new GetProductsByCategoryQuery
        {
            Category = category,
            Page = page,
            Size = size,
            Order = order
        };

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponseWithData<GetProductsByCategoryResponse>
        {
            Success = true,
            Message = "Products retrieved successfully",
            Data = _mapper.Map<GetProductsByCategoryResponse>(result)
        });
    }

    /// <summary>
    /// Updates a product
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateProductRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateProductCommand>(request);
        command.Id = id;

        try
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateProductResponse>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = _mapper.Map<UpdateProductResponse>(result)
            });
        }
        catch (Domain.Exceptions.NotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Updates the stock quantity of a product
    /// </summary>
    [HttpPut("{id}/stock")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateStockResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateStock([FromRoute] Guid id, [FromBody] UpdateStockRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateStockRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new UpdateStockCommand
        {
            Id = id,
            Quantity = request.Quantity
        };

        try
        {
            var success = await _mediator.Send(command, cancellationToken);

            var userEmail = GetCurrentUserEmail();

            return Ok(new ApiResponseWithData<UpdateStockResponse>
            {
                Success = true,
                Message = $"Stock updated successfully by {userEmail}",
                Data = new UpdateStockResponse
                {
                    Success = true,
                    Message = "Stock updated successfully",
                    ProductId = id,
                    Quantity = request.Quantity
                }
            });
        }
        catch (Domain.Exceptions.NotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Adds a rating to a product
    /// </summary>
    [HttpPost("{id}/rating")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseWithData<AddRatingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddRating([FromRoute] Guid id, [FromBody] AddRatingRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new AddRatingRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new AddRatingCommand
        {
            Id = id,
            Rating = request.Rating
        };

        try
        {
            var success = await _mediator.Send(command, cancellationToken);

            var product = await _mediator.Send(new GetProductByIdQuery { Id = id }, cancellationToken);

            var userEmail = GetCurrentUserEmail();

            return Ok(new ApiResponseWithData<AddRatingResponse>
            {
                Success = true,
                Message = $"Rating added successfully by {userEmail}",
                Data = new AddRatingResponse
                {
                    Success = true,
                    Message = "Rating added successfully",
                    ProductId = id,
                    NewRating = product.Rating.Rate,
                    RatingCount = product.Rating.Count
                }
            });
        }
        catch (Domain.Exceptions.NotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);

        if (!success)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Product not found"
            });

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Product deleted successfully"
        });
    }
}