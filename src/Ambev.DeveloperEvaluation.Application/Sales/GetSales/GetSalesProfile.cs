using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Profile for mapping between domain entities and DTOs for retrieving sales list.
/// </summary>
public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<Sale, GetSaleListItemResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
}