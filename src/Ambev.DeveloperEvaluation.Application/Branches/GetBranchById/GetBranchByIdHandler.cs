using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;

/// <summary>
/// Handler for getting a branch by ID
/// </summary>
public class GetBranchByIdHandler : IRequestHandler<GetBranchByIdQuery, BranchDetailVm>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetBranchByIdHandler
    /// </summary>
    public GetBranchByIdHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get branch by ID query
    /// </summary>
    public async Task<BranchDetailVm> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        return _mapper.Map<BranchDetailVm>(branch);
    }
}
