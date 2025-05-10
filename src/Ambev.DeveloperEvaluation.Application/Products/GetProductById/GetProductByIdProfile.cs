using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductById;

public class GetProductByIdProfile : Profile
{
    public GetProductByIdProfile()
    {
        CreateMap<Product, ProductDetailVm>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingVm
            {
                Rate = src.RatingRate,
                Count = src.RatingCount
            }));
    }
}
