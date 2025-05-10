using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Mapping profile for CancelSale feature.
/// </summary>
public class CancelSaleProfile : Profile
{
    public CancelSaleProfile()
    {
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}
