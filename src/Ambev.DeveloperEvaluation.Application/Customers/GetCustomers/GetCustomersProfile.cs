using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

public class GetCustomersProfile : Profile
{
    public GetCustomersProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}
