using AutoMapper;
using Example.Domain.Addresses.Views;
using Example.Domain.CustomerEmployerMappings.Views;
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
                .ForMember(c=>c.Address, option => option.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.Address != null)
                    {
                        dst.Address = 
                        dst.Address = new AddressView
                        {
                            Id = src.Address.Id,
                            Street = src.Address.Street,
                            Suburb = src.Address.Suburb,
                            PostalCode = src.Address.PostalCode,
                            State = src.Address.State,
                            Country = src.Address.Country
                        };
                    }
                })
                ;

            CreateMap<CustomerEmployerMapping, GetCustomerEmployerMappingDetailedDataView>()
                .ForMember(cem => cem.CustomerName, entity => entity.MapFrom(x => (x.Customer != null) ? x.Customer.FirstName : "<Customer Is Null>"))
                .ForMember(cem => cem.EmployerName , entity => entity.MapFrom(x => (x.Employer != null) ? x.Employer.Name : "<Employer Is Null>"))
                ;
        }
    }
}
