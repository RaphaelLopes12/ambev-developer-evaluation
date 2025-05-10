using Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;

public class GetBranchesProfile : Profile
{
    public GetBranchesProfile()
    {
        CreateMap<BranchDetailVm, GetBranchResponse>();
        CreateMap<BranchDto, BranchListItem>();
    }
}
