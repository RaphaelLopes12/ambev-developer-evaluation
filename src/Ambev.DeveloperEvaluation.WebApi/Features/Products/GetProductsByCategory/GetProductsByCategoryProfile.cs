using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;

public class GetProductsByCategoryProfile: Profile
{
    public GetProductsByCategoryProfile()
    {
        CreateMap<ProductsByCategoryVm, GetProductsByCategoryResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
}
