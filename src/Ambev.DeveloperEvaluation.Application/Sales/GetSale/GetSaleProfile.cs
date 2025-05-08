using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Profile for mapping between domain entities and DTOs for retrieving a sale.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<SaleItem, GetSaleItemResult>();

        }
    }
}
