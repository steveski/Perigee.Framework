using AutoMapper;
using Example.Entities;
using ExampleRestApi.Contract;

namespace Example.Mappings
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
