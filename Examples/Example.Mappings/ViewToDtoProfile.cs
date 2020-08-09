using AutoMapper;
using Example.Domain.Addresses.Views;
using ExampleRestApi.Contract;

namespace Example.Mappings
{
    public class ViewToDtoProfile : Profile
    {
        public ViewToDtoProfile()
        {
            CreateMap<GetAddressView, AddressDto>();
        }
    }
}
