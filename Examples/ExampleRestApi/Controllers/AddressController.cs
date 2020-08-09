using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleRestApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using AutoMapper;
    using Example.Domain.Addresses.Commands;
    using Example.Domain.Addresses.Queries;
    using Example.Domain.Addresses.Views;
    using Example.Entities;
    using ExampleRestApi.Contract;
    using Perigee.Framework.Base.Transactions;

    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IProcessQueries _queries;
        private readonly IProcessCommands _commands;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IProcessQueries queries, IProcessCommands commands, IMapper mapper, ILogger<AddressController> logger)
        {
            _queries = queries;
            _commands = commands;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto address, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<AddressDto, CreateAddressCommand>(address);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _commands.Execute(command, cancellationToken).ConfigureAwait(false);
            if (command.CreatedEntity == null) return BadRequest();

            var contract = _mapper.Map<Address, AddressDto>(command.CreatedEntity);

            return CreatedAtAction(
                "GetAddress",
                new { id = command.CreatedEntity.Id },
                contract);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses(CancellationToken cancellationToken)
        {
            var addresses = await _queries.Execute(new AddressesBy(), cancellationToken).ConfigureAwait(false);

            var contract = _mapper.Map<IEnumerable<GetAddressView>, IEnumerable<AddressDto>>(addresses);

            return Ok(contract);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAddress([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var query = new AddressesBy { Id = id };
            var addresses = await _queries.Execute(query, cancellationToken).ConfigureAwait(false);

            var address = addresses.FirstOrDefault();
            if (address == null) return NotFound();

            var contract = _mapper.Map<GetAddressView, AddressDto>(address);

            return Ok(contract);
        }
    }
}
