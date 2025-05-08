using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Profile for mapping between domain entities and DTOs for cancelling sale items.
/// </summary>
public class CancelSaleItemProfile : Profile
{
    public CancelSaleItemProfile()
    {
        // Mapeamento da entidade SaleItem para o resultado
        CreateMap<SaleItem, CancelSaleItemResult>()
            .ForMember(dest => dest.Success, opt => opt.Ignore())
            .ForMember(dest => dest.SaleId, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.NewTotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Message, opt => opt.Ignore());
    }
}
