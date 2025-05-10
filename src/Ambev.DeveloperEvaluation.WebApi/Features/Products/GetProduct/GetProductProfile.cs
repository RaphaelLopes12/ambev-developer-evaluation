using Ambev.DeveloperEvaluation.Application.Products.GetProductById;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<ProductRatingVm, ProductRatingResponse>();
        CreateMap<ProductDetailVm, GetProductResponse>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResponse
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));

        CreateMap<ProductDto, ProductListItem>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResponse
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
