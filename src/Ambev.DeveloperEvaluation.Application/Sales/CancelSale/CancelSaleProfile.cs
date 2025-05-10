using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleProfile : Profile
{
    public CancelSaleProfile()
    {
        CreateMap<Sale, CancelSaleResult>()
            .ForMember(dest => dest.Success, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(_ => "Sale cancelled successfully"));
    }
}
