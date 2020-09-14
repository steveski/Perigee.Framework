using Example.Portal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Example.Portal.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerDataService _customerDataService;

        public CustomersController(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerDataService.getCustomers();

            return Ok();
        }
    }
}
