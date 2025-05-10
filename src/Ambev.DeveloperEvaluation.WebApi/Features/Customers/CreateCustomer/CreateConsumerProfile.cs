using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

public class CreateConsumerProfile : Profile
{
	public CreateConsumerProfile()
	{
        CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
        CreateMap<CreateCustomerResult, CreateCustomerResponse>();
    }
}
