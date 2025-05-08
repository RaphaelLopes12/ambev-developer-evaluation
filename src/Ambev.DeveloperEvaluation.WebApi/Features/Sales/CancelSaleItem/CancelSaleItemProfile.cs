using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// Mapping profile for CancelSaleItem feature.
/// </summary>
public class CancelSaleItemProfile : Profile
{
    public CancelSaleItemProfile()
    {
        CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
    }
}
