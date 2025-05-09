using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCostumerProfile : Profile
{
    public GetCostumerProfile()
    {
        CreateMap<Customer, CustomerDetailDto>();
    }
}
