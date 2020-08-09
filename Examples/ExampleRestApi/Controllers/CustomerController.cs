using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleRestApi.Controllers
{
    using System.Threading;
    using AutoMapper;
    using Example.Domain.Customers.Commands;
    using Example.Domain.Customers.Queries;
    using Example.Entities;
    using ExampleRestApi.Contract;
    using Perigee.Framework.Base.Transactions;

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IProcessQueries _queries;
        private readonly IProcessCommands _commands;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IProcessQueries queries, IProcessCommands commands, IMapper mapper, ILogger<CustomerController> logger)
        {
            _queries = queries;
            _commands = commands;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customer, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CustomerDto, CreateCustomerCommand>(customer);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _commands.Execute(command, cancellationToken).ConfigureAwait(false);
            if (command.CreatedEntity == null) return BadRequest();

            var contract = _mapper.Map<Customer, CustomerDto>(command.CreatedEntity);

            return CreatedAtAction(
                "GetCustomer",
                new { id = command.CreatedEntity.Id },
                contract);
        }

        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
        {
            var customers = await _queries.Execute(new CustomersBy(), cancellationToken).ConfigureAwait(false);

            var contract = customers.Select(customer => new CustomerDto
            {
                id = customer.Id,
                firstName = customer.FirstName,
                lastName = customer.LastName,
                emailAddress = customer.EmailAddress
            });

            return Ok(contract);
        }

        // GET: api/customer/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var query = new CustomersBy { Id = id };
            var customers = await _queries.Execute(query, cancellationToken).ConfigureAwait(false);
            
            var customer = customers.FirstOrDefault();
            if (customer == null) return NotFound();

            var contract = new CustomerDto
            {
                id = customer.Id,
                firstName = customer.FirstName,
                lastName = customer.LastName,
                emailAddress = customer.EmailAddress
            };

            return Ok(contract);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(CustomerDto customer, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var custUpdateCommand = _mapper.Map<CustomerDto, UpdateCustomerCommand>(customer);

            await _commands.Execute(custUpdateCommand, cancellationToken);

            return Ok();
        }


    }
}
