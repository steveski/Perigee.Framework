using FluentAssertions;
using AutoMapper;
using Example.Domain.CustomerEmployerMappings.Queries;
using Example.Entities;
using Example.Mappings;
using MockQueryable.NSubstitute;
using NSubstitute;
using Perigee.Framework.Base.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Example.Domain.Customers.Queries;

namespace ExampleConsoleApp.UnitTests
{
    using System;
    using System.Linq.Expressions;

    public class GetCustomerEmployerMappings
    {
        protected IReadEntities _readDb;
        protected IMapper _mapper;
        private readonly IEnumerable<Address> _addresses;
        private readonly IEnumerable<Customer> _customers;
        private readonly IEnumerable<Employer> _employers;
        private readonly IEnumerable<CustomerEmployerMapping> _customerEmployerMappings;

        public GetCustomerEmployerMappings()
        {
            _readDb = Substitute.For<IReadEntities>();
            _mapper = new MapperConfiguration(c =>
                c.AddProfiles(
                    new List<Profile>
                    {
                        new CommandToEntityProfile(),
                        new DtoToCommandProfile(),
                        new EntityToDtoProfile(),
                        new EntityToViewProfile(),
                    }
                ))
                .CreateMapper();

            var anAddress = new Address
            {
                Id = 1,
                Street = "street",
                Suburb = "suburb",
                State = "state",
                Country = "country",
                PostalCode = "postcode",
            };
            _addresses = new List<Address>()
            {
                anAddress
            };
            var mockAddressQueryable = _addresses.AsQueryable().BuildMock();
            _readDb.Query<Address>().Returns(mockAddressQueryable);

            var aCust = new Customer
            {
                Id = 101,
                FirstName = "first name",
                LastName = "last name",
                EmailAddress = "my.email@address",
                AddressId = anAddress.Id
            };
            _customers = new List<Customer>()
            {
                aCust
            };
            var mockCustomerQueryable = _customers.AsQueryable().BuildMock();
            _readDb.Query<Customer>().Returns(mockCustomerQueryable);
            //_readDb.Query<Customer>(Arg.Any<IEnumerable<Expression<Func<Customer, object>>>>()).Returns(mockCustomerQueryable);

            var anEmp = new Employer
            {
                Id = 1001,
                Name = "employer",
            };
            _employers = new List<Employer>()
            {
                anEmp
            };
            var mockEmployerQueryable = _employers.AsQueryable().BuildMock();
            _readDb.Query<Employer>().Returns(mockEmployerQueryable);

            var aMapping = new CustomerEmployerMapping
            {
                Id = 999,
                CustomerId = aCust.Id,
                EmployerId = anEmp.Id
            };
            _customerEmployerMappings = new List<CustomerEmployerMapping>()
            {
                aMapping
            };
            var mockCustomerEmployerMappingQueryable = _customerEmployerMappings.AsQueryable().BuildMock();
            _readDb.Query<CustomerEmployerMapping>().Returns(mockCustomerEmployerMappingQueryable);

        }

        [Fact]
        public async Task GetCustomerEmployerMappingsDetailed()
        {
            var custQuery = new Example.Domain.Customers.Queries.CustomersBy();
            var custsut = new HandleCustomerByQuery(_readDb, _mapper);
            var custresult = await custsut.Handle(custQuery, CancellationToken.None);
            custresult.Should().HaveCount(1);
            //custresult.ElementAt(0).Address.Street.Should().NotBeNull(); // TODO: Address keeps coming back as NULL. it was populated on one debug but not sure why

            // TODO: Having issues getting mock sorted for the .Query(includes) method. SOrt this out
            //var query = new Example.Domain.CustomerEmployerMappings.Queries.GetCustomerEmployerMappings();
            //var sut = new HandleCustomerEmployerMappingsWithIncludeByQuery(_readDb, _mapper);
            //var result = await sut.Handle(query, CancellationToken.None);
            //result.Should().HaveCount(1);
        }
    }
}
