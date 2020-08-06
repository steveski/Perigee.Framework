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
            CreateMap<Customer, GetCustomerWithAddressView>();
            CreateMap<Customer, GetCustomerWithAddressView>()
                .ForPath(v => v.Address.Street, e => e.MapFrom(x => x.Address.Street))
                .ForPath(v => v.Address.Suburb, e => e.MapFrom(x => x.Address.Suburb))
                .ForPath(v => v.Address.PostalCode, e => e.MapFrom(x => x.Address.PostalCode))
                .ForPath(v => v.Address.State, e => e.MapFrom(x => x.Address.State))
                .ForPath(v => v.Address.Country, e => e.MapFrom(x => x.Address.Country))
                ;
        }
    }
}
