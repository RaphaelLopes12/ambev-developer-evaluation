using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales;

/// <summary>
/// Profile for mapping between Sale entities and CreateSale request/response objects.
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes AutoMapper mappings for sale creation.
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ConstructUsing(cmd => new Sale(
                cmd.Number,
                cmd.Date,
                cmd.CustomerId,
                cmd.CustomerName,
                cmd.BranchId,
                cmd.BranchName
            ));

        CreateMap<CreateSaleItemDto, SaleItem>();

        CreateMap<SaleItem, CreateSaleItemResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}
