using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;

public class GetBranchByIdProfile : Profile
{
    public GetBranchByIdProfile()
    {
        CreateMap<Branch, BranchDetailVm>();
    }
}
