using AutoMapper;
using Example.Domain.Addresses.Commands;
using Example.Domain.Customers.Commands;
using Example.Entities;

namespace Example.Mappings
{
    public class CommandToEntityProfile : Profile
    {
        public CommandToEntityProfile()
        {
            CreateMap<CreateAddressCommand, Address>();
            CreateMap<CreateAddressCommandWithTransientDbContext, Address>();
            CreateMap<CreateCustomerCommand, Customer>();
            CreateMap<CreateCustomerCommandWithTransientDbContext, Customer>();
        }
    }
}
