using AutoMapper;
using Example.Domain.Addresses.Commands;
using Example.Domain.Customers.Commands;
using ExampleRestApi.Contract;

namespace Example.Mappings
{
    public class DtoToCommandProfile : Profile
    {
        public DtoToCommandProfile()
        {
            CreateMap<CustomerDto, CreateCustomerCommand>();
            CreateMap<AddressDto, CreateAddressCommand>();
        }
    }
}
