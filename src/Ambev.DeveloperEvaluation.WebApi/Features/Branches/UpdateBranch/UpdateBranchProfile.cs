using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

public class UpdateBranchProfile : Profile
{
    public UpdateBranchProfile()
    {
        CreateMap<UpdateBranchRequest, UpdateBranchCommand>();
        CreateMap<UpdateBranchResult, UpdateBranchResponse>();
    }
}
