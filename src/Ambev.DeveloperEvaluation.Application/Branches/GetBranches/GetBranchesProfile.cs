using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

public class GetBranchesProfile : Profile
{
    public GetBranchesProfile()
    {
        CreateMap<Branch, BranchDto>();
    }
}
