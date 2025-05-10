using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<GetProductsRequest, GetProductsQuery>();
        CreateMap<ProductsListVm, GetProductsResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));

        CreateMap<ProductDto, ProductListItem>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResponse
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
