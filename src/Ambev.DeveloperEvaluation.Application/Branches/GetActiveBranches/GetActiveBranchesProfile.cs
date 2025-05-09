using System.Text;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetActiveBranches;

public class GetActiveBranchesProfile : Profile
{
    public GetActiveBranchesProfile()
    {
        CreateMap<Branch, ActiveBranchDto>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => $"{src.City}, {src.State}"))
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => GetContactInfo(src)));
    }

    private string GetContactInfo(Branch branch)
    {
        var contactBuilder = new StringBuilder();

        if (!string.IsNullOrEmpty(branch.Phone))
        {
            contactBuilder.Append(branch.Phone);
        }

        if (!string.IsNullOrEmpty(branch.Email))
        {
            if (contactBuilder.Length > 0)
            {
                contactBuilder.Append(" | ");
            }

            contactBuilder.Append(branch.Email);
        }

        return contactBuilder.ToString();
    }
}
