using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetActiveBranches;

/// <summary>
/// Handler for getting all active branches
/// </summary>
public class GetActiveBranchesHandler : IRequestHandler<GetActiveBranchesQuery, List<ActiveBranchDto>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetActiveBranchesHandler
    /// </summary>
    public GetActiveBranchesHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get active branches query
    /// </summary>
    public async Task<List<ActiveBranchDto>> Handle(GetActiveBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepository.GetAllAsync();
        var activeBranches = branches.Where(b => b.IsActive).ToList();
        return _mapper.Map<List<ActiveBranchDto>>(activeBranches);
    }
}
