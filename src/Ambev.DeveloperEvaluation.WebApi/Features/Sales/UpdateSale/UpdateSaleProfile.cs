using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Mapping profile for UpdateSale feature.
/// </summary>
public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        // Request → Command mappings
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemDto>();

        // Result → Response mappings
        CreateMap<UpdateSaleResult, UpdateSaleResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<UpdatedSaleItemDto, UpdatedSaleItemResponse>();
    }
}
