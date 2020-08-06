using AutoMapper;
using Example.Entities;
using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Domain.CustomerEmployerMappings.Commands
{
    public class CreateCustomerEmployerMappingCommand : BaseCreateEntityCommand<CustomerEmployerMapping>
    {
        public int CustomerId { get; set; }
        public int EmployerId { get; set; }
    }

    public class HandleCreateCustomerEmployerMappingCommand : IHandleCommand<CreateCustomerEmployerMappingCommand>
    {
        private readonly IWriteEntities _db;
        private readonly IMapper _mapper;

        public HandleCreateCustomerEmployerMappingCommand(IWriteEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task Handle(CreateCustomerEmployerMappingCommand command, CancellationToken cancellationToken)
        {
            var cem = _mapper.Map<CreateCustomerEmployerMappingCommand, CustomerEmployerMapping>(command);

            _db.Create(cem);
            command.CreatedEntity = cem;

            return Task.CompletedTask;
        }
    }
}
