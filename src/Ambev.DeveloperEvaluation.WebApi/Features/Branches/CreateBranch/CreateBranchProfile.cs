using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

public class CreateBranchProfile : Profile
{
    public CreateBranchProfile()
    {
        CreateMap<CreateBranchRequest, CreateBranchCommand>();
        CreateMap<CreateBranchResult, CreateBranchResponse>();
    }
}
