using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

/// <summary>
/// Handler for getting all branches
/// </summary>
public class GetBranchesHandler : IRequestHandler<GetBranchesQuery, List<BranchDto>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetBranchesHandler
    /// </summary>
    public GetBranchesHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get branches query
    /// </summary>
    public async Task<List<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepository.GetAllAsync();
        return _mapper.Map<List<BranchDto>>(branches);
    }
}
