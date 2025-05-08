using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Profile for mapping between Application and WebAPI for GetSales.
/// </summary>
public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        // Request to Query
        CreateMap<GetSalesRequest, GetSalesQuery>();

        // Result to Response
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<GetSaleListItemResult, GetSalesItemResponse>();
    }
}