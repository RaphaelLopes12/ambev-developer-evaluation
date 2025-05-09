using Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

public class GetBranchProfile : Profile
{
    public GetBranchProfile()
    {
        CreateMap<BranchDetailVm, GetBranchResponse>();
        CreateMap<BranchDto, BranchListItem>();
    }
}
