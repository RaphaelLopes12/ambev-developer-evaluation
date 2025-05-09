using Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;

public class GetCostumersProfile : Profile
{
    public GetCostumersProfile()
    {
        CreateMap<CustomerDto, CustomerListItem>();
    }
}
