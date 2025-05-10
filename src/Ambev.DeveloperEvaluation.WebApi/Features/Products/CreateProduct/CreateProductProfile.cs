using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating != null
                ? new Ambev.DeveloperEvaluation.Application.Products.CreateProduct.RatingDto
                {
                    Rate = src.Rating.Rate,
                    Count = src.Rating.Count
                }
                : null));

        CreateMap<CreateProductResult, CreateProductResponse>();
    }
}
