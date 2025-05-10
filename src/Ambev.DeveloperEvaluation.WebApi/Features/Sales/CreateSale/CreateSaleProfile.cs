using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Web API and Application layers for Sale creation.
/// </summary>
public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, CreateSaleItemDto>();

        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
    }
}
