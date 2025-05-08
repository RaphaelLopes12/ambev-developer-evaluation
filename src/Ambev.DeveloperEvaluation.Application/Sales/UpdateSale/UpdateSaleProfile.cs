using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between domain entities and DTOs for updating sales.
/// </summary>
public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        // Domain → DTO mappings
        CreateMap<Sale, UpdateSaleResult>()
            .ForMember(dest => dest.Success, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(_ => "Sale updated successfully"))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleItem, UpdatedSaleItemDto>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
    }
}