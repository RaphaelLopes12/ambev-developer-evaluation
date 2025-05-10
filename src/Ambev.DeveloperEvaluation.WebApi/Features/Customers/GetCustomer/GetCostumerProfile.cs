using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

public class GetCostumerProfile : Profile
{
    public GetCostumerProfile()
    {
        CreateMap<CustomerDetailDto, GetCustomerResponse>();
    }
}
