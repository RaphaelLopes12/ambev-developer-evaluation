using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new RatingDto
            {
                Rate = src.RatingRate,
                Count = src.RatingCount
            }));
    }
}
