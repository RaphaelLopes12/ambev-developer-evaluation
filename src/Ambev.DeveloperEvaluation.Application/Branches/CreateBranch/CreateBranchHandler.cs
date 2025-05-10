using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Handler for creating a branch
/// </summary>
public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, CreateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the CreateBranchHandler
    /// </summary>
    public CreateBranchHandler(IBranchRepository branchRepository, IMediator mediator)
    {
        _branchRepository = branchRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the create branch command
    /// </summary>
    public async Task<CreateBranchResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = new Branch(
            request.Name,
            request.Address,
            request.City,
            request.State,
            request.ZipCode,
            request.Phone,
            request.Email
        );

        var validationResult = branch.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult);
        }

        var createdBranch = await _branchRepository.AddAsync(branch);

        await _mediator.Publish(new BranchCreatedNotification
        {
            Id = createdBranch.Id,
            Name = createdBranch.Name
        }, cancellationToken);

        return new CreateBranchResult
        {
            Id = createdBranch.Id,
            Name = createdBranch.Name,
            City = createdBranch.City,
            State = createdBranch.State
        };
    }
}