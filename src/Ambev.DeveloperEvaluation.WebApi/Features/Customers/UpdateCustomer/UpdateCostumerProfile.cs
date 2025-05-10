using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

public class UpdateCostumerProfile : Profile
{
    public UpdateCostumerProfile()
    {
        CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();
        CreateMap<UpdateCustomerResult, UpdateCustomerResponse>();
    }
}
