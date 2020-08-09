using AutoMapper;
using Example.Entities;
using Example.Mappings;
using MockQueryable.NSubstitute;
using NSubstitute;
using Perigee.Framework.Base.Database;
using System.Collections.Generic;
using System.Linq;

namespace ExampleConsoleApp.UnitTests
{
    public class TestBase
    {
        protected IReadEntities _readDb;
        protected IWriteEntities _writeDb;
        protected IMapper _mapper;
        private readonly IEnumerable<Address> _addresses;
        private readonly IEnumerable<Customer> _customers;
        private readonly IEnumerable<Employer> _employers;
        private readonly IEnumerable<CustomerEmployerMapping> _customerEmployerMappings;

        public TestBase()
        {
            _readDb = Substitute.For<IReadEntities>();
            _writeDb = Substitute.For<IWriteEntities>();
            _mapper = new MapperConfiguration(c =>
                c.AddProfiles(
                    new List<Profile>
                    {
                        new CommandToEntityProfile(),
                        new DtoToCommandProfile(),
                        new EntityToDtoProfile(),
                        new EntityToViewProfile(),
                        new ViewToDtoProfile()
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
            _writeDb.Query<Address>().Returns(mockAddressQueryable);

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
            _writeDb.Query<Customer>().Returns(mockCustomerQueryable);
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
            _writeDb.Query<Employer>().Returns(mockEmployerQueryable);

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
            _writeDb.Query<CustomerEmployerMapping>().Returns(mockCustomerEmployerMappingQueryable);
        }
    }
}
