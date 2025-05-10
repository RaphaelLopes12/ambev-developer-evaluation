using Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.DeactivateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Branches.GetActiveBranches;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranches;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.ActivateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeactivateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetActiveBranches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches;

/// <summary>
/// Controller for managing branch operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BranchesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of BranchesController
    /// </summary>
    public BranchesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new branch
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateBranchResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateBranchRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateBranchCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateBranchResponse>
        {
            Success = true,
            Message = "Branch created successfully",
            Data = _mapper.Map<CreateBranchResponse>(result)
        });
    }

    /// <summary>
    /// Gets a branch by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBranch([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBranchByIdQuery { Id = id }, cancellationToken);

        if (result == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Branch not found"
            });

        return Ok(new ApiResponseWithData<GetBranchResponse>
        {
            Success = true,
            Message = "Branch retrieved successfully",
            Data = _mapper.Map<GetBranchResponse>(result)
        });
    }

    /// <summary>
    /// Gets all branches
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetBranchesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBranches(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetBranchesQuery(), cancellationToken);

        return Ok(new ApiResponseWithData<GetBranchesResponse>
        {
            Success = true,
            Message = "Branches retrieved successfully",
            Data = new GetBranchesResponse
            {
                Branches = _mapper.Map<List<BranchListItem>>(result)
            }
        });
    }

    /// <summary>
    /// Gets all active branches
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<GetActiveBranchesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveBranches(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetActiveBranchesQuery(), cancellationToken);

        return Ok(new ApiResponseWithData<GetActiveBranchesResponse>
        {
            Success = true,
            Message = "Active branches retrieved successfully",
            Data = new GetActiveBranchesResponse
            {
                Branches = _mapper.Map<List<ActiveBranchItem>>(result)
            }
        });
    }

    /// <summary>
    /// Updates a branch
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateBranch([FromRoute] Guid id, [FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateBranchRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateBranchCommand>(request);
        command.Id = id;

        try
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateBranchResponse>
            {
                Success = true,
                Message = "Branch updated successfully",
                Data = _mapper.Map<UpdateBranchResponse>(result)
            });
        }
        catch (Domain.Exceptions.NotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Branch with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Activates a branch
    /// </summary>
    [HttpPatch("{id}/activate")]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponseWithData<ActivateBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ActivateBranch([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _mediator.Send(new ActivateBranchCommand { Id = id }, cancellationToken);

            return Ok(new ApiResponseWithData<ActivateBranchResponse>
            {
                Success = true,
                Message = "Branch activated successfully",
                Data = new ActivateBranchResponse
                {
                    Success = true,
                    Message = "Branch activated successfully"
                }
            });
        }
        catch (Domain.Exceptions.NotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Branch with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Deactivates a branch
    /// </summary>
    [HttpPatch("{id}/deactivate")]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponseWithData<DeactivateBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeactivateBranch([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _mediator.Send(new DeactivateBranchCommand { Id = id }, cancellationToken);

            return Ok(new ApiResponseWithData<DeactivateBranchResponse>
            {
                Success = true,
                Message = "Branch deactivated successfully",
                Data = new DeactivateBranchResponse
                {
                    Success = true,
                    Message = "Branch deactivated successfully"
                }
            });
        }
        catch (Domain.Exceptions.NotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Branch with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Deletes a branch
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireManagerRole")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteBranch([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new DeleteBranchCommand { Id = id }, cancellationToken);

        if (!success)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Branch not found"
            });

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Branch deleted successfully"
        });
    }
}