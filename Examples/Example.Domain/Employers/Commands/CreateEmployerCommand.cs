using AutoMapper;
using Example.Entities;
using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Domain.Employers.Commands
{
    public class CreateEmployerCommand : BaseCreateEntityCommand<Employer>
    {
        public string Name { get; set; }
    }

    public class HandleCreateEmployerCommand : IHandleCommand<CreateEmployerCommand>
    {
        private readonly IWriteEntities _db;
        private readonly IMapper _mapper;

        public HandleCreateEmployerCommand(IWriteEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task Handle(CreateEmployerCommand command, CancellationToken cancellationToken)
        {
            var emp = _mapper.Map<CreateEmployerCommand, Employer>(command);

            _db.Create(emp);
            command.CreatedEntity = emp;

            return Task.CompletedTask;
        }
    }
}
