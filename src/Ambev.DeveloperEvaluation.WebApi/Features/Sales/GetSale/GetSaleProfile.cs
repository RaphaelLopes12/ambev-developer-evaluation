using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// AutoMapper profile for mapping between Web API and Application layer for GetSale.
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleProfile"/> class.
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();
    }
}
