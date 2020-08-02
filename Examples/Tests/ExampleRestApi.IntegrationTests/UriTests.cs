namespace ExampleRestApi.IntegrationTests
{
    using System.Threading.Tasks;
    using Example.Domain.Customers.Commands;
    using ExampleRestApi.Contract;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class UriTests : IClassFixture<WebApplicationFactory<ExampleRestApi.Startup>>
    {
        private string _mediaType = "application/json";

        private readonly WebApplicationFactory<Startup> _factory;

        public UriTests(WebApplicationFactory<ExampleRestApi.Startup> factory)
        {
            _factory = factory;
        }
        
        [Theory]
        [InlineData("/customer")]
        public async Task GetEndpoints(string uri)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(uri).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().StartWith(_mediaType);

        }

        [Fact]
        public async Task PostEndpoints()
        {
            string uri = "/customer";
            var client = _factory.CreateClient();

            var command = new CreateCustomerCommand
            {
                FirstName = "Herbert",
                LastName = "Scrackle",
                EmailAddress = "herby@home.com"
            };

            var httpContent = command.ToHttpContent(_mediaType);

            var response = await client.PostAsync(uri, httpContent).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().StartWith(_mediaType);
            
            var theDto = response.Content.ReadAsJsonAsync<CustomerDto>();
            theDto.Id.Should().BeGreaterThan(0);


        }



    }
}
