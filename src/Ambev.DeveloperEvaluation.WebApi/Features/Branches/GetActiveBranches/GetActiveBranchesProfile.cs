using Ambev.DeveloperEvaluation.Application.Branches.GetActiveBranches;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetActiveBranches;

public class GetActiveBranchesProfile : Profile
{
    public GetActiveBranchesProfile()
    {
        CreateMap<ActiveBranchDto, ActiveBranchItem>()
         .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
         .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contact));
    }
}
