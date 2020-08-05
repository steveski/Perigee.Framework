using AutoMapper;
using Example.Domain.Addresses.Views;
using Example.Domain.Customers.Views;
using Example.Entities;

namespace Example.Mappings
{
    public class EntityToViewProfile : Profile
    {
        public EntityToViewProfile()
        {
            CreateMap<Address, GetAddressView>();
            CreateMap<Customer, GetCustomerView>();
            CreateMap<Customer, GetCustomerWithAddressView>()
                .ForMember(v => v.Street, e => e.MapFrom(x => x.Address.Street))
                .ForMember(v => v.Suburb, e => e.MapFrom(x => x.Address.Suburb))
                .ForMember(v => v.PostalCode, e => e.MapFrom(x => x.Address.PostalCode))
                .ForMember(v => v.State, e => e.MapFrom(x => x.Address.State))
                .ForMember(v => v.Country, e => e.MapFrom(x => x.Address.Country))
                ;
        }
    }
}
