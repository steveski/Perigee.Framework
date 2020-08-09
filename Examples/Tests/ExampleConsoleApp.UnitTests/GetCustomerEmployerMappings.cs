using Example.Domain.Customers.Queries;
using FluentAssertions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExampleConsoleApp.UnitTests
{
    using Example.Domain.Customers.Commands;

    public class GetCustomerEmployerMappings : TestBase
    {
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
